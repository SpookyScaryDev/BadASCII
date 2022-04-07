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

    abstract class TextBuffer {
        public abstract int           width { get; }
        public abstract int           height { get; }

        public abstract void          Create(int width, int height);
        public abstract void          Blit(char character, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false);
        public abstract void          Print();


        public abstract char          GetCharacterAt(Vector2i position);
        public abstract ConsoleColor  GetColourAt(Vector2i position);


        public void Clear(char clearChar = ' ', ConsoleColor clearColour = ConsoleColor.Gray) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Blit(clearChar, new Vector2i(i, j), clearColour);
                }
            }
        }

        public void Blit(string message, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false) {
            Vector2i pos = new Vector2i();
            pos.x = position.x - 1;
            pos.y = position.y;

            for (int i = 0; i < message.Length; i++) {
                pos.x++;

                if (message[i] == '\n' && pos.y + 1 < height) {
                    pos.y += 1;
                    pos.x = position.x - 1;
                    continue;
                }

                if (!(pos.x >= width || pos.y >= height) && message[i] != '\r') {
                    Blit(message[i], pos, colour, alpha);
                }
            }
        }

        public void Blit(TextBuffer buffer, Vector2i position, bool alpha = false) {
            Vector2i pos = new Vector2i();
            Vector2i posInBuffer = new Vector2i();
            for (int i = 0; i < buffer.height; i++) {
                for (int j = 0; j < buffer.width; j++) {
                    posInBuffer.x = j;
                    posInBuffer.y = i;

                    pos.x = position.x + j;
                    pos.y = position.y + i;

                    if (pos.x < width && pos.y < height) {
                        Blit(buffer.GetCharacterAt(posInBuffer), pos, buffer.GetColourAt(posInBuffer), alpha);
                    }
                }
            }
        }

    }

}
