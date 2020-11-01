using System;

namespace BrainMRIDesktop
{
    public class MedImage
    {
        public DateTime CreationDate { get; private set; }
        public string Interval { get; private set; }
        public string ImgType { get; private set; }
        public string Age { get; private set; }
        public string Gender { get; private set; }


        public MedImage(DateTime creationDate, string interval, string imgType, string age, string gender)
        {
            CreationDate = creationDate;
            Interval = interval;
            ImgType = imgType;
            Age = age;
            Gender = gender;
        }

    }
}
