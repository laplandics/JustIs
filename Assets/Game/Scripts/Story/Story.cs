public static class Story
{
    public static class Ancestors
    {
        public static string DefaultExamineText1 => "Золотая статуэтка. С виду ничего особенного: двое мужчин стоят с торжествующим видом, " +
                                                  "один из них победоносно поднял руку над головой.";
        public static string DefaultExamineText1Alt => "Золотая статуэтка.";
        public static string DefaultExamineText2 => "А, быть может, это призыв к продолжению борьбы?";
        public static string DefaultExamineText3 => "И что бы за борьба это была...";

        public static string DefaultExamineChoice1 => "*Осмотреть*";
        public static string DefaultExamineChoice2 => "*Отложить*";

        public static string DefaultExamineAfterChoice1Text1 => "Ничего примечательного.";
        public static string DefaultExamineAfterChoice1Choice1 => "Ясно.";
    }

    public static class Person
    {
        public static string FirstMeetText1 => "Неизвестный с мешком на голове. Выглядит напуганным.";
        
        public static string FirstMeetChoice1 => "'Привет...'";
        public static string FirstMeetChoice2 => "*Уйти*";

        public static string FirstMeetAfterChoice1Text1 => "Человек не отвечает.";
    }
}