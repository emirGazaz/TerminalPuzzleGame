namespace Game{
    class FirstSequence{
        
        public const string PlayerAlias = "ShadowRunner";
        public static void flow(){

            Console.Clear();

            string filePath = "script0.txt";  

            GameMain.ReadScript(filePath);

            Console.Clear();

        }

    }
}