using System; 
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BadASCII {

    class FastTextBuffer : ITextBuffer {
        // Windows API code based on https://stackoverflow.com/a/2754674.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputW(
            SafeFileHandle hConsoleOutput, 
            CharInfo[] lpBuffer, 
            Coord dwBufferSize, 
            Coord dwBufferCoord, 
            ref SmallRect lpWriteRegion
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord {
            public short X;
            public short Y;

            public Coord(short X, short Y) {
              this.X = X;
              this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion {
            [FieldOffset(0)] public ushort UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        CharInfo[] mBuffer;
        SmallRect  mBufferRect;
        SafeFileHandle mHandle;

        public FastTextBuffer(int width, int height) {
            mHandle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            Create(width, height);
        }

        public int width {
            get;
            private set;
        }

        public int height {
            get;
            private set;
        }

        public char GetCharacterAt(Vector2i position) {
            return (char)mBuffer[width * position.y + position.x].Char.UnicodeChar;
        }

        public ConsoleColor GetColourAt(Vector2i position) {
            return (ConsoleColor)mBuffer[width * position.y + position.x].Attributes;
        }

        public void Create(int width, int height) {
            mBuffer = new CharInfo[width * height];
            mBufferRect = new SmallRect() { Left = 0, Top = 0, Right = (short)width, Bottom = (short)height };
            this.width = width;
            this.height = height;
            Clear();
        }

        public void Clear(char clearChar = ' ', ConsoleColor clearColour = ConsoleColor.Gray) {
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    Blit(clearChar, new Vector2i(i, j), clearColour);
                }
            }
        }

        public void Blit(char character, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false) {
            if (position.x < width && position.y < height && position.x >= 0 && position.y >= 0) {
                if ((!alpha) || alpha && character != ' ') {
                    mBuffer[width * position.y + position.x].Attributes = (short)colour;
                    mBuffer[width * position.y + position.x].Char.UnicodeChar = (ushort)character;
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
            WriteConsoleOutputW(mHandle, mBuffer,
              new Coord() { X = (short)width, Y = (short)height },
              new Coord() { X = 0, Y = 0 },
              ref mBufferRect
            );
        }
    }

}
