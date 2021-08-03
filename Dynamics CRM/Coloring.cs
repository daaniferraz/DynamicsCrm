using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamics_CRM
{
    public  class Coloring
    {

        public enum Situation
        {
            Normal = 0,
            Espacamento = 1,
            Erro = 2
            
        }

        public void ChangeColor(Situation situation)
        {
            switch (situation)
            {
                case Situation.Espacamento:
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(" ");
                    break;
                case Situation.Erro:
                    //Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case Situation.Normal:
                default:
                    Console.ResetColor();
                    break;
            }
        }

    }
}
