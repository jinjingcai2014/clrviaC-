using System;
using System.Windows.Forms;
using System.IO;

//声明一个委托类型，他的实例引用一个方法
internal delegate void Feedback(Int32 value);

public sealed class Program{
	public static void Main()
    {
        StaticDelegateDemo();
        InstanceDelegateDemo();
        ChainDelegateDemo1(new Program());
        ChainDelegateDemo2(new Program());
    }
    private static void StaticDelegateDemo()
    {
        Console.WriteLine("---------static Delegate Demo ---------");
        Counter(1, 3, null);
        Counter(1, 3, new Feedback(Program.FeedbackToConsole));
        Counter(1, 3, new Feedback(FeedbackToMsgBox));
        Console.WriteLine();
    }

    private static void InstanceDelegateDemo()
    {
        Console.WriteLine("----------instance Delegate Demo-----");
        Program p = new Program();
        Counter(1, 3, new Feedback(p.FeedbackToFile));
        Console.WriteLine();
    }


    private static void ChainDelegateDemo1(Program p)
    {
        Console.WriteLine("-----------chain Delegate Demo1------");
        Feedback fb1 = new Feedback(FeedbackToConsole);
        Feedback fb2 = new Feedback(FeedbackToMsgBox);
        Feedback fb3 = new Feedback(FeedbackToFile);

        Feedback fbChain = null;
        fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
        fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
        Counter(1, 2, fbChain);
    }

    private static void ChainDelegateDemo2(Program p)
    {
        Console.WriteLine("--------Chain Delegate Demo2----");
        Feedback fb1 = new Feedback(FeedbackToConsole);
        Feedback fb2 = new Feedback(FeedbackToMsgBox);
        Feedback fb3 = new Feedback(FeedbackToFile);

        Feedback fbChain = null;
        fbChain += fb1;
        fbChain += fb2;
        fbChain += fb3;
        Counter(1, 2, fbChain);

        Console.WriteLine();
        fbChain -= new Feedback(FeedbackToMsgBox);
        Counter(1, 2, fbChain);
    }
    private static void Counter(Int32 from, Int32 to, Feedback fb)
    {
        for(Int32 val = from; val < to; val++)
        {
            if (fb != null)
                fb(val);
        }
    }

    private static void FeedbackToConsole(Int32 value)
    {
        Console.WriteLine("Item = " + value);
    }

    private static void FeedbackToMsgBox(Int32 value)
    {
        MessageBox.Show("Item=" + value);
    }

    private  void FeedbackToFile(Int32 value)
    {
        using(StreamWriter sw = new StreamWriter("Status", true))
        {
            sw.WriteLine("Item= " + value);
        }
    }
}


