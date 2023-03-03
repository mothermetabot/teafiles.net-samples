using TeaTime;

namespace TeaFiles.Net.Test
{
    public struct Data
    {
        public double X { get; set; }

        public double Y { get; set; }

        public long Id { get; set; }

        public Time Time { get; set; }

        public override string ToString()
        {
            return $"{Id} : {Time.NetTime.ToFileTimeUtc()} -- X:{X}, Y:{Y}";
        }
    }
}
