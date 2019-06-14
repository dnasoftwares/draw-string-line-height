using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NUnit.Framework;
using draw_string_line_height;

namespace Tests
{
    public class GraphicsExtensionTests
    {
        protected Bitmap SampleBmp;
        protected Graphics GraphicsInstance;
        protected Font MonoSpaceFont;
        protected float CharacterWidth;

        [OneTimeSetUp]
        public void Setup()
        {
            this.SampleBmp = new Bitmap(200, 200);
            this.GraphicsInstance = Graphics.FromImage(this.SampleBmp);
            this.GraphicsInstance.Clear(Color.White);
            this.MonoSpaceFont = new Font(FontFamily.GenericMonospace, 20);

            this.CharacterWidth = GraphicsInstance.MeasureString("D", this.MonoSpaceFont).Width;
        }

        [Test]
        /// <summary>
        /// Passing an empty string to get wrapper lines should return an empty
        /// string array
        /// </summary>
        [TestCase("")]
        [TestCase(" ")] // One space
        [TestCase("            ")] // A lot of spaces
        [TestCase("     \t     ")] // A lot of spaces and a tab character
        public void GetWrappedLines_Empty_String(string s)
        {
            string[] lines = this.GraphicsInstance.GetWrappedLines(s, MonoSpaceFont, 20).ToArray();
            Assert.AreEqual(0, lines.Length, "Must return an empty lines array for an empty string");            
        }

        [Test]
        /// <summary>
        /// A single character must result in a single line
        /// </summary>
        public void GetWrappedLines_One_Char()
        {
            string[] lines = this.GraphicsInstance.GetWrappedLines("a", this.MonoSpaceFont).ToArray();
            Assert.AreEqual(1, lines.Length, "Must return a line array of length 1 for a single chracter");
        }

        [Test]
        /// <summary>
        /// When passed a max width that is smaller than the size of a single, the returned string enumerable
        /// is expected to have a length equal to the number of words in the given string becase the split occurs at every word
        /// (NOT every character)
        /// </summary>
        [TestCase("a")]
        [TestCase("Test 123")]
        [TestCase("I love good coffee")]
        public void GetWrappedLines_MaxWidth_Smaller_Than_CharWidth(string s)
        {
            int wordCount = s.Split((char[]) null).Length;
            string[] lines = this.GraphicsInstance.GetWrappedLines(s, this.MonoSpaceFont, this.CharacterWidth - 1).ToArray();
            Assert.AreEqual(wordCount, lines.Length);
        }

        /// <summary>
        /// When using the default maxWidth (Infinity), an array of length 1 should be returned for a non empty or blank string
        /// </summary>
        public void GetWrappedLines_Default_Width()
        {
            // Haiku inspired by https://ubuntuforums.org/showthread.php?t=1101510&p=6929375#post6929375 :)
            string s = "php how i hate thee, your objects and libs are messy, now hope it doesn't fail if you pass String.Empty";
            string[] lines = this.GraphicsInstance.GetWrappedLines(s, this.MonoSpaceFont).ToArray();
            Assert.AreEqual(1, lines.Length);
        }
    }
}