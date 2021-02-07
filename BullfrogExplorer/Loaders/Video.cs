using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BullfrogExplorer.Loaders
{

    //word = ushort: 	0 à 65 535	Entier 16 bits non signé
    //dword = uint: de 0 à 4 294 967 295	Entier 32 bits non signé
    //https://www.compuphase.com/flic.htm
    //https://github.com/aseprite/flic/blob/master/flic.h

    class Video
    {

        private bool debug = false;

        // 128 bytes
        public struct Header
        {
            int size;          //4 bytes /* Size of FLIC including this header */
            int type;        //2 bytes /* File type 0xAF11 (Fli files), 0xAF12 (FLC 8bit colour depth), 0xAF30 (FLC Huffman/BWT comp.), 0xAF44 (FLC no 8bits colour depth), ... */
            ushort frames;        /* Number of frames in first segment */
            ushort width;         /* FLIC width in pixels */
            ushort height;        /* FLIC height in pixels */
            ushort depth;         /* Bits per pixel (usually 8) */
            ushort flags;         /* Set to zero or to three */
            uint speed;         /* Delay between frames */
            ushort reserved1;     /* Set to zero */
            uint created;       /* Date of FLIC creation (FLC only) */
            uint creator;       /* Serial number or compiler id (FLC only) */
            uint updated;       /* Date of FLIC update (FLC only) */
            uint updater;       /* Serial number (FLC only), see creator */
            ushort aspect_dx;     /* Width of square rectangle (FLC only) */
            ushort aspect_dy;     /* Height of square rectangle (FLC only) */
            ushort ext_flags;     /* EGI: flags for specific EGI extensions */
            ushort keyframes;     /* EGI: key-image frequency */
            ushort totalframes;   /* EGI: total number of frames (segments) */
            uint req_memory;      /* EGI: maximum chunk size (uncompressed) */
            ushort max_regions;   /* EGI: max. number of regions in a CHK_REGION chunk */
            ushort transp_num;    /* EGI: number of transparent levels */
            byte[] reserved2;     /* 24Set to zero */
            uint oframe1;         /* Offset to frame 1 (FLC only) */
            uint oframe2;         /* Offset to frame 2 (FLC only) */
            byte[] reserved3;     /* 40 Set to zero */
        }


        struct Frame
        {
            uint size; //4 bytes
            ushort type; //2 bytes Always hexadecimal F1FA
            ushort chunks; //2 bytes
            byte[] expand; // 8 bytes Space for future enhancements.All zeros

        }

        struct Chunk
        {
            uint size;           /* Size of the chunk, always 64 */
            ushort type;           /* Chunk type: 3 */
            short center_x;       /* Coordinates of the cel centre or origin */
            short center_y;
            ushort stretch_x;      /* Stretch amounts */
            ushort stretch_y;
            ushort rot_x;          /* Rotation in x-axis (always 0) */
            ushort rot_y;          /* Rotation in y-axis (always 0) */
            ushort rot_z;          /* z-axis rotation, 0-5760=0-360 degrees */
            ushort cur_frame;      /* Current frame in cel file */
            byte[] reserved1;   /* 2 Reserved, set to 0 */
            ushort transparent;    /* Transparent colour index */
            ushort[] overlay;    /* 16 Frame overlay numbers */
            byte[] reserved2;    /* 6 Reserved, set to 0 */
        }
        /*
                number name      meaning
             11        FLI_COLOR Compressed color map
             12        FLI_LC Line compressed -- the most common type
                                 of compression for any but the first
                                 frame.Describes the pixel difference
                                 from the previous frame.
             13        FLI_BLACK Set whole screen to color 0 (only occurs
                                 on the first frame).
             15        FLI_BRUN Bytewise run-length compression -- first
                                frame only
             16        FLI_COPY Indicates uncompressed 64000 bytes soon
                                 to follow.For those times when
                                 compression just doesn't work!
        The compression schemes are all byte-oriented.If the compressed data ends up being an odd length a single pad byte is inserted so that the FLI_COPY's always start at an even address for faster DMA.

           3       CEL_DATA             registration and transparency
           4       COLOR_256            256-level colour palette
           7       DELTA_FLC (FLI_SS2)  delta image, word oriented RLE
          11       COLOR_64             64-level colour palette
          12       DELTA_FLI (FLI_LC)   delta image, byte oriented RLE
          13       BLACK                full black frame (rare)
          15       BYTE_RUN (FLI_BRUN)  full image, byte oriented RLE
          16       FLI_COPY             uncompressed image (rare)
          18       PSTAMP               postage stamp (icon of the first frame)
          25       DTA_BRUN             full image, pixel oriented RLE
          26       DTA_COPY             uncompressed image
          27       DTA_LC               delta image, pixel oriented RLE
          31       LABEL                frame label
          32       BMP_MASK             bitmap mask
          33       MLEV_MASK            multilevel mask
          34       SEGMENT              segment information
          35       KEY_IMAGE            key image, similar to BYTE_RUN / DTA_BRUN
          36       KEY_PAL              key palette, similar to COLOR_256
          37       REGION               region of frame differences
          38       WAVE                 digitized audio
          39       USERSTRING           general purpose user data
          40       RGN_MASK             region mask
          41       LABELEX              extended frame label (includes symbolic name)
          42       SHIFT                scanline delta shifts (compression)
          43       PATHMAP              path map (segment transitions)

          0xF100   PREFIX_TYPE          prefix chunk
          0xF1E0   SCRIPT_CHUNK         embedded "Small" script
          0xF1FA   FRAME_TYPE           frame chunk
          0xF1FB   SEGMENT_TABLE        segment table chunk
          0xF1FC   HUFFMAN_TABLE        Huffman compression table chunk

*/

        public Header header;


        public Video()
        {

        }


        public void ToggleDebug()
        {
            debug = !debug;
        }


        public bool LoadFile(string filename)
        {


            if (!File.Exists(filename))
            {
                return false;
            }
            else
            {


                Console.WriteLine(" - LoadContent() Reading file: " + filename);
                //using (FileStream fs = File.OpenRead(path))
                using (FileStream fs = File.OpenRead(filename))
                {
                    int b=0;
                    byte[] buffer = new byte[128];
                    fs.Read(buffer, 0, buffer.Length);
                    int filesize = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int type = buffer[b++] + (buffer[b++] << 8);
                    int frames = buffer[b++] + (buffer[b++] << 8);
                    int width = buffer[b++] + (buffer[b++] << 8);
                    int height = buffer[b++] + (buffer[b++] << 8);
                    int depth = buffer[b++] + (buffer[b++] << 8);
                    int flags = buffer[b++] + (buffer[b++] << 8);
                    int created = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int creator = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int updated = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int updater = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int aspect_dx = buffer[b++] + (buffer[b++] << 8);
                    int aspect_dy = buffer[b++] + (buffer[b++] << 8);
                    int ext_flags = buffer[b++] + (buffer[b++] << 8);
                    int keyframes = buffer[b++] + (buffer[b++] << 8);
                    int totalframes = buffer[b++] + (buffer[b++] << 8);
                    int req_memory = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int max_regions = buffer[b++] + (buffer[b++] << 8);
                    int transp_num = buffer[b++] + (buffer[b++] << 8);
                    b = b + 23;
                    int oframe1 = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);
                    int oframe2 = buffer[b++] + (buffer[b++] << 8) + (buffer[b++] << 16) + (buffer[b++] << 24);



                    if (debug)
                    {
                        Console.WriteLine("filesize : " + filesize + " type: " + type.ToString("X") + " frames: " + frames +
                            " width: " + width + " height: " + height + " depth: " + depth + " flags: " + flags + 
                            " date: " + created + " creator: " + creator + " updated: " + updated + " updater: " + updater +
                            " aspect_dx: " + aspect_dx + " aspect_dy: " + aspect_dy + " extflags: " + ext_flags +
                            " keyframes: " + keyframes + " totalframe: "+totalframes + " req_mem: " +req_memory + " transnum:" + transp_num +
                            " oframe1: " + oframe1 + " oframe2: " + oframe2);
                    }


                    return true;
                }
            }
        }


    }
}
