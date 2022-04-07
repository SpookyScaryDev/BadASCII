using System; 
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BadASCII {

    class FastTextBuffer : TextBuffer {
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

        public override int width {
            get {
                return mBufferRect.Right;
            }
        }

        public override int height {
            get {
                return mBufferRect.Bottom;
            }
        }

        public override char GetCharacterAt(Vector2i position) {
            return (char)mBuffer[width * position.y + position.x].Char.UnicodeChar;
        }

        public override ConsoleColor GetColourAt(Vector2i position) {
            return (ConsoleColor)mBuffer[width * position.y + position.x].Attributes;
        }

        public override void Create(int width, int height) {
            mBuffer = new CharInfo[width * height];
            mBufferRect = new SmallRect() { Left = 0, Top = 0, Right = (short)width, Bottom = (short)height };
            Clear();
        }

        public override void Blit(char character, Vector2i position, ConsoleColor colour = ConsoleColor.White, bool alpha = false) {
            if (position.x < width && position.y < height && position.x >= 0 && position.y >= 0) {
                if ((!alpha) || alpha && character != ' ') {
                    mBuffer[width * position.y + position.x].Attributes = (short)colour;
                    mBuffer[width * position.y + position.x].Char.UnicodeChar = (ushort)character;
                }
            }

        }

        public override void Print() {
            // Create a copy so the rects values don't change if actual rectangle that was used is different.
            SmallRect rect = mBufferRect;
            WriteConsoleOutputW(mHandle, mBuffer,
                new Coord() { X = (short)width, Y = (short)height },
                new Coord() { X = 0, Y = 0 },
                ref rect
            );
        }
    }

}
