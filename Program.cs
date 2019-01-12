using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Core
{
    class Program
    {


        static void Main(string[] args)
        {
            //if (args.Length != 1)
            //{
            //    WriteLine("Please provide exactly 1 argument - the text to split.");
            //    return;
            //}

            //var messageInput = args[0];
            var messageInput = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut iaculis sollicitudin porta. Aenean at semper augue, " +
                "sed hendrerit augue. Sed eu tincidunt nunc, sed vestibulum ipsum. Vestibulum aliquet ex felis, vitae cursus odio vulputate nec. " +
                "Nulla vitae dictum ipsum, sit amet pharetra lorem. Interdum et malesuada fames ac ante ipsum primis in faucibus.";

            var configuration = new SplitterConfiguration();

            var splitter = new Splitter(configuration);

            var tweets = splitter.Split(messageInput);

            for (int i = 0; i < tweets.Count(); i++)
            {
                var tweet = tweets.ElementAt(i);
                WriteLine($"Printing Tweet {i + 1} of {tweets.Count()}");
                WriteLine($"\"{tweet}\"");
                WriteLine();
            }
        }
    }
}
