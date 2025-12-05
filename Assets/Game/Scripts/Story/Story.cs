public static class Story
{
    public static class Ancestors
    {
        public static string FirstExamineText1 => "Золотая статуэтка. С виду ничего особенного: двое мужчин стоят с торжествующим видом, " +
                                                  "один из них победоносно поднял руку над головой.";
        public static string FirstExamineText2 => "А, быть может, это призыв к продолжению борьбы?";
        public static string FirstExamineText3 => "И что бы за борьба это была...";

        public static string FirstExamineChoice1 => "*Осмотреть*";
        public static string FirstExamineChoice2 => "*Отложить*";

        public static string FirstExamineAfterChoice1Text1 => "Ничего примечательного.";
        public static string FirstExamineAfterChoice1Choice1 => "Ясно.";
    }

    public static class Person
    {
        public static string FirstMeetText1 => "Неизвестный с мешком на голове. Выглядит напуганным.";
        
        public static string FirstMeetChoice1 => "'Привет...'";
        public static string FirstMeetChoice2 => "*Уйти*";

        public static string FirstMeetAfterChoice1Text1 => "Человек не отвечает.";
    }
}