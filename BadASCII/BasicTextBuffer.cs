using System; 

namespace BadASCII {

    class BasicTextBuffer : ITextBuffer {
        private char[,] mBuffer;
        private int[,] mColours;

        public BasicTextBuffer(int width, int height) {
            Create(width, height);
        }

        public int width {
            get {
                return mBuffer.GetLength(1);
            }
        }

        public int height {
            get {
                return mBuffer.GetLength(0);
            }
        }

        public char GetCharacterAt(Vector2i position) {
            return mBuffer[position.y, position.x];
        }

        public ConsoleColor GetColourAt(Vector2i position) {
            return (ConsoleColor)mColours[position.y, position.x];
        }

        public void Create(int width, int height) {
            mBuffer = new char[height, width];
            mColours = new int[height, width];
            Clear();
        }

        public void Clear(char clearChar = ' ', ConsoleColor clearColour = ConsoleColor.Gray) {
            for (int i = 0; i < mBuffer.GetLength(0); i++) {
                for (int j = 0; j < mBuffer.GetLength(1); j++) {
                    mBuffer[i, j] = clearChar;
                    mColours[i, j] = (int)clearColour;
                }
            }
        }

        public void Blit(char character, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false) {
            if (position.x < width && position.y < height && position.x >= 0 && position.y >= 0) {
                if ((!alpha) || alpha && character != ' ') {
                    mBuffer[position.y, position.x] = character;
                    mColours[position.y, position.x] = (int)colour;
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

        public void Blit(ITextBuffer buffer, Vector2i position, bool alpha = false) {
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

        public void Print() {
            ConsoleColor prevColour = GetColourAt(new Vector2i(0, 0));
            ConsoleColor currentColor = ConsoleColor.White;
            ConsoleColor nextColor = GetColourAt(new Vector2i(0, 0));
            string colourLine = "";
            Vector2i pos = new Vector2i();
            Vector2i nextPos = new Vector2i();

            for (int i = 0; i < mBuffer.GetLength(0); i++) {
                for (int j = 0; j < mBuffer.GetLength(1); j++) {
                    pos.x = j;
                    pos.y = i;

                    nextPos.x = j + 1;
                    nextPos.y = i;
                    if (nextPos.x > (width - 1)) {
                        nextPos.x = 0;
                        nextPos.y++;
                        if (nextPos.y > (height - 1)) {
                            nextPos = pos;
                        }
                    }

                    currentColor = GetColourAt(pos);
                    nextColor = GetColourAt(nextPos);
                    colourLine += GetCharacterAt(pos);

                    if (currentColor != nextColor) {
                        Console.ForegroundColor = currentColor;
                        Console.Write(colourLine);
                        colourLine = "";
                    } else if ((i == (height - 1) && j == (width - 1))) {
                        Console.ForegroundColor = currentColor;
                        Console.Write(colourLine);
                        colourLine = "";
                    }

                    prevColour = currentColor;
                }
                if (currentColor != nextColor) {
                    Console.Write("\n");
                } else {
                    colourLine += "\n";
                }
            }
        }
    }

}