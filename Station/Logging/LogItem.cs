namespace Station.Logging
{
    class LogItem
    {
        public LogItem()
        {
            Background = "White";
            Foreground = "Black";

        }


        public string Message { get; set; }
        public string Time { get; set; }
        public string Background { get; set; }
        public string Foreground { get; set; }



    }
}
