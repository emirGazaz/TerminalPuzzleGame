using System.Dynamic;

namespace Game{
    class IntroScreen{
        private static int state = 0;
        public static void displayIntroText(){

            Console.Clear();
            GameMain.write(">Type 'start' to play : ");
            string input = Console.ReadLine() ?? string.Empty;
            input = input?.Trim() ?? string.Empty;

            while(!(input?.Equals("start", StringComparison.OrdinalIgnoreCase) ?? false)){
                Console.Clear();
                GameMain.write(">Type 'start' to play : ");
                input = Console.ReadLine() ?? string.Empty;
            }

            state = 0;
            return;
        }

        public static void setState(int value){
            state=value;
        }
    }
}