using System;

namespace BadASCII {

    class Constants {
        public const char PIXEL = '█';
    }

    struct Vector2i {
        public int x;
        public int y;

        public Vector2i(int xPos, int yPos) {
            x = xPos;
            y = yPos;
        }
    }

    struct Vector3i {
        public int x;
        public int y;
        public int z;

        public Vector3i(int xPos, int yPos, int zPos) {
            x = xPos;
            y = yPos;
            z = zPos;
        }
    }

    interface ITextBuffer {
        int           width { get; }
        int           height { get; }

        void          Create(int width, int height);
        void          Clear(char clearChar = ' ', ConsoleColor clearColour = ConsoleColor.Gray);

        char          GetCharacterAt(Vector2i position);
        ConsoleColor  GetColourAt(Vector2i position);

        void          Blit(char character, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false);
        void          Blit(string message, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false);
        void          Blit(ITextBuffer buffer, Vector2i position, bool alpha = false);

        void          Print();
    }

}
