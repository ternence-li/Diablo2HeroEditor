﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Diablo2FileFormat.Sections
{
    /// <summary>
    /// The fixed length section is the part of the file that includes everything prior to the stats section.
    /// </summary>
    public class HeaderSection : IDiablo2FileSection
    {
        public byte[] Data { get; }
        public bool IsChanged { get; set; }
        public int Size => 335;

        protected virtual uint Diablo2FileSignature => 0xAA55AA55;
        protected virtual int VersionOffset => 0x04;
        protected virtual int FileSizeOffset => 0x08;
        protected virtual int ChecksumOffset => 0x0C;
        protected virtual int CharacterNameOffset => 0x14;
        protected virtual int CharacterStatusOffset => 0x24;
        protected virtual int CharacterProgressionOffset => 0x25;
        protected virtual int ClassOffset => 0x28;
        protected virtual int SkillSectionLengthOffset => 0x2A;

        public HeaderSection(byte[] data)
        {
            Data = new byte[Size];
            Array.Copy(data, Data, Size);
        }

        public string CharacterName
        {
            get
            {
                return Encoding.ASCII.GetString(Data, CharacterNameOffset, 16).Trim(new[] { '\0' });
            }
            set
            {
                if (IsValidCharacterName(value))
                {
                    var nameBytes = Encoding.ASCII.GetBytes(value);
                    Array.Copy(nameBytes, 0, Data, CharacterNameOffset, nameBytes.Length);

                    IsChanged = true;
                }
                // else throw?
            }
        }

        protected bool IsValidCharacterName(string name)
        {
            bool isValid = false;

            if (name.Length >= 2 && name.Length <= 15)
            {
                isValid = new Regex("[a-zA-Z]+[a-zA-Z-_]?[a-zA-Z]+").IsMatch(name); //new Regex("[a-zA-Z]*[-_]?[a-zA-Z]*").IsMatch(name); &&
            }

            return isValid;
        }

        public int SkillSectionLength => Data[SkillSectionLengthOffset] + 2; // +2 for the section delimiter

        public int FileSize
        {
            get { return BitConverter.ToInt32(Data, FileSizeOffset); }
            set
            {
                var bytes = BitConverter.GetBytes(value);
                Array.Copy(bytes, 0, Data, FileSizeOffset, bytes.Length);

                IsChanged = true;
            }
        }

        public HeroClass HeroClass
        {
            get { return (HeroClass)Data[ClassOffset]; }
            set
            {
                if (value != HeroClass)
                {
                    Data[ClassOffset] = (byte)value;
                    IsChanged = true;
                }
            }
        }
    }
}