using System.Collections.Generic;

namespace BullfrogExplorer.Data
{
    class DataFiles
    {

        #region ** structs **
        public struct AnimationsFiles
        {
            public int index;
            public string StartFile;
            public string FramesFile;
            public string ElementsFile;
            public string LabelsFile;

            public AnimationsFiles(int i, string s, string f, string e, string t)
            {
                index = i;
                StartFile = s;
                FramesFile = f;
                ElementsFile = e;
                LabelsFile = t;
            }


        }

        public struct TextsFile
        {
            public int index;
            public string file;
            public string name;

            public TextsFile(int i, string p, string n)
            {
                file = p;
                index = i;
                name = n;
            }
        }

        public struct SoundsFile
        {
            public int index;
            public string tabFile;
            public string datFile;
            public string name;

            public SoundsFile(int i, string tf, string sf, string n)
            {
                tabFile = tf;
                datFile = sf;
                index = i;
                name = n;
            }
        }

        public struct MusicsFile
        {
            public int index;
            public string tabFile;
            public string datFile;
            public string name;

            public MusicsFile(int i, string tf, string sf, string n)
            {
                tabFile = tf;
                datFile = sf;
                index = i;
                name = n;
            }
        }

        public struct PaletteFile
        {
            public int index;
            public string file;
            public string name;

            public PaletteFile(int i, string p, string n)
            {
                file = p;
                index = i;
                name = n;
            }
        }

        public struct SpritesFile
        {
            public int index;
            public string tabFile;
            public string datFile;
            public int paletteIndex;
            public string paletteName;
            public string name;

            public SpritesFile(int i, string t, string d, int p, string pn, string n)
            {
                index = i;
                tabFile = t;
                datFile = d;
                paletteIndex = p;
                paletteName = pn;
                name = n;
            }
        }

        public struct VideoFile
        {
            public int index;
            public string file;

            public VideoFile(int i, string s)
            {
                index = i;
                file = s;
            }
        }

        public struct BackgroundFile
        {
            public int index;
            public string file;
            public string palette;

            public BackgroundFile(int i, string s, string p)
            {
                index = i;
                file = s;
                palette = p;
            }
        }

        #endregion

        #region ** Lists **
        public static List<PaletteFile> paletteFiles = new List<PaletteFile>();
        public static List<SpritesFile> spritesFiles = new List<SpritesFile>();
        public static List<SoundsFile> soundsFiles = new List<SoundsFile>();
        public static List<MusicsFile> musicsFiles = new List<MusicsFile>();
        public static List<AnimationsFiles> animationsFiles = new List<AnimationsFiles>();
        public static List<VideoFile> videoFiles = new List<VideoFile>();
        public static List<BackgroundFile> backgroundFiles = new List<BackgroundFile>();
        public static List<TextsFile> textsFiles = new List<TextsFile>();
        #endregion

        static DataFiles()
        {
            #region ** PALETTES **

            int i = 0;
            paletteFiles.Add(new PaletteFile(i++, "MAWPAL-0.DAT", "MAWPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MNGPAL-0.DAT", "MNGPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MPALETTE.DAT", "MPALETTE"));
            paletteFiles.Add(new PaletteFile(i++, "MRSPAL-0.DAT", "MRSPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MSTPAL-0.DAT", "MSTPAL"));
            paletteFiles.Add(new PaletteFile(i++, "BUSPAL.DAT", "BUSPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MGLPAL-0.DAT", "MGLPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MNGPAL-0.DAT", "MNGPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MSTAP-0.DAT", "MSTAP"));
            paletteFiles.Add(new PaletteFile(i++, "TAKPAL.DAT", "TAKPAL"));
            paletteFiles.Add(new PaletteFile(i++, "MSTAPAL-.DAT", "MSTAPAL"));
            #endregion

            #region ** LANG **
            i = 0;
            textsFiles.Add(new TextsFile(i++, "LANG0-0.DAT", "ENGLISH"));
            textsFiles.Add(new TextsFile(i++, "LANG1-0.DAT", "GERMAN"));
            textsFiles.Add(new TextsFile(i++, "LANG2-0.DAT", "ITALIANO"));
            textsFiles.Add(new TextsFile(i++, "LANG3-0.DAT", "SPANISH"));
            textsFiles.Add(new TextsFile(i++, "LANG4-0.DAT", "FRENCH"));
            #endregion

            #region ** SPRITES **
            i = 0;
            spritesFiles.Add(new SpritesFile(i++, "MSPR-0.TAB", "MSPR-0.DAT", 1, "MPALETTE", "MSPR"));
            spritesFiles.Add(new SpritesFile(i++, "HPANEL-0.TAB", "HPANEL-0.DAT", 1, "MPALETTE", "HPANEL"));
            spritesFiles.Add(new SpritesFile(i++, "MBLK-0.TAB", "MBLK-0.DAT", 1, "MPALETTE", "MBLK"));
            spritesFiles.Add(new SpritesFile(i++, "MCUP-0.TAB", "MCUP-0.DAT", 1, "MAWPAL", "MCUP"));
            spritesFiles.Add(new SpritesFile(i++, "MPANEL-0.TAB", "MPANEL-0.DAT", 1, "MPALETTE", "MPANEL"));
            spritesFiles.Add(new SpritesFile(i++, "MRSSPR-0.TAB", "MRSSPR-0.DAT", 1, "MRSPAL", "MRSSPR"));
            spritesFiles.Add(new SpritesFile(i++, "MSTSPR-0.TAB", "MSTSPR-0.DAT", 1, "MSTPAL", "MSTSPR"));
            spritesFiles.Add(new SpritesFile(i++, "MFONT-0.TAB", "MFONT-0.DAT", 1, "MPALETTE", "MFONT"));
            spritesFiles.Add(new SpritesFile(i++, "MPLAY-0.TAB", "MPLAY-0.DAT", 1, "MPALETTE", "MPLAY"));
            spritesFiles.Add(new SpritesFile(i++, "MPOINTER.TAB", "MPOINTER.DAT", 1, "MPALETTE", "MPOINTER"));
            spritesFiles.Add(new SpritesFile(i++, "MREQ-0.TAB", "MREQ-0.DAT", 1, "MPALETTE", "MREQ"));
            spritesFiles.Add(new SpritesFile(i++, "MFONT-0.TAB", "MFONT-0.DAT", 1, "WHITE", "MFONT2"));
            spritesFiles.Add(new SpritesFile(i++, "MAUSPR-0.TAB", "MAUSPR-0.DAT", 1, "MPALETTE", "MAUSPR")); // AUCTION SPRITES
            spritesFiles.Add(new SpritesFile(i++, "MHAND-0.TAB", "MHAND-0.DAT", 1, "MNGPAL", "MHAND"));
            #endregion

            #region ** SOUNDS **
            i = 0;
            soundsFiles.Add(new SoundsFile(i++, "SNDS0-0.TAB", "SNDS0-1.DAT", "SNDS0-0"));
            soundsFiles.Add(new SoundsFile(i++, "SNDS0-1.TAB", "SNDS0-1.DAT", "SNDS0-1"));
            soundsFiles.Add(new SoundsFile(i++, "SNDS0-2.TAB", "SNDS0-2.DAT", "SNDS0-2"));
            soundsFiles.Add(new SoundsFile(i++, "SNDS1-0.TAB", "SNDS1-0.DAT", "SNDS1-0"));
            soundsFiles.Add(new SoundsFile(i++, "SNDS1-1.TAB", "SNDS1-1.DAT", "SNDS1-1"));
            soundsFiles.Add(new SoundsFile(i++, "SNDS1-2.TAB", "SNDS1-2.DAT", "SNDS1-2"));
            #endregion

            #region ** MUSIC **
            i = 0;
            musicsFiles.Add(new MusicsFile(i++, "MUSIC0-0.TAB", "MUSIC0-0.DAT", "MUSIC0-0"));
            musicsFiles.Add(new MusicsFile(i++, "MUSIC0-1.TAB", "MUSIC0-1.DAT", "MUSIC0-1"));
            musicsFiles.Add(new MusicsFile(i++, "MUSIC0-2.TAB", "MUSIC0-2.DAT", "MUSIC0-2"));
            musicsFiles.Add(new MusicsFile(i++, "MUSIC1-0.TAB", "MUSIC1-0.DAT", "MUSIC1-0"));
            musicsFiles.Add(new MusicsFile(i++, "MUSIC1-1.TAB", "MUSIC1-1.DAT", "MUSIC1-1"));
            musicsFiles.Add(new MusicsFile(i++, "MUSIC1-2.TAB", "MUSIC1-2.DAT", "MUSIC1-2"));
            #endregion

            #region ** ANIMATIONS **
            i = 0;
            animationsFiles.Add(new AnimationsFiles(i++, "MSTA-0.ANI", "MFRA-0.ANI", "MELE-0.ANI", "MEDIT-0.ANI"));
            animationsFiles.Add(new AnimationsFiles(i++, "MDSTA-0.ANI", "MDFRA-0.ANI", "MDELE-0.ANI", "MDEDIT-0.ANI"));
            //Je ne connais pas la différence entre les deux
            // MSELE-0.ANI  ??????? Même taille que MELE-0.ANI
            #endregion

            #region ** VIDEO (NOT IMPLEMENTED) **
            /*
                        i = 0;
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.000"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.002"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.003"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.004"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.006"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.007"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.008"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.009"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.012"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.013"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.014"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.019"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.021"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.026"));
                        videoFiles.Add(new VideoFile(i++, path + "RIDEANI.028"));
                        */
            #endregion

            #region ** BACKGROUNDS **
            i = 0;
            backgroundFiles.Add(new BackgroundFile(i++, "BUSTED.DAT", "BUSPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MAUCT-0.DAT", "MPALETTE"));
            backgroundFiles.Add(new BackgroundFile(i++, "MAWAR0-0.DAT", "MAWPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MAWAR1-0.DAT", "MAWPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MGLOBE-0.DAT", "MGLPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MIDLAND.DAT", "MSTAP"));
            backgroundFiles.Add(new BackgroundFile(i++, "MMAP-0.DAT", "MPALETTE"));
            backgroundFiles.Add(new BackgroundFile(i++, "MMENU-0.DAT", "MPALETTE"));
            backgroundFiles.Add(new BackgroundFile(i++, "MMENU-1.DAT", "MPALETTE"));
            backgroundFiles.Add(new BackgroundFile(i++, "MNEG-0.DAT", "MNGPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MRES-0.DAT", "MRSPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "MSHARE-0.DAT", "MPALETTE"));
            backgroundFiles.Add(new BackgroundFile(i++, "MSTATE-0.DAT", "MSTAP"));
            backgroundFiles.Add(new BackgroundFile(i++, "MSTOCK-0.DAT", "MSTPAL"));
            backgroundFiles.Add(new BackgroundFile(i++, "TAKOVER.DAT", "TAKPAL"));
            #endregion

        }
    }
}
