using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NNTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] inputs = new double[100];
            RandomUtil.RandomizeDoubleArray(inputs);
            NetWork n = new NetWork(new int[] {4, 5,5,2});
            var tmp = n.Compute(inputs);

            Console.WriteLine("inputs");
            foreach (var i in inputs)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("outputs");
            foreach (var i in tmp)
            {
                Console.WriteLine(i);
            }
        }
    }

    class NetWork
    {
        private Newron[][] Newrons;

        public NetWork(int[] layerCounts)
        {
            Newrons = new Newron[layerCounts.Length][];
            for (int i = 0; i < layerCounts.Length; i++)
            {
                Newrons[i] = new Newron[layerCounts[i]];
            }
            for (int i = 0; i < layerCounts.Length; i++)
            {
                for (int j = 0; j < layerCounts[i]; j++)
                {
                    if (i == 0)
                    {
                        Newrons[i][j] = new Newron(1, i);
                    }
                    else
                    {
                        Newrons[i][j] = new Newron(layerCounts[i - 1], i);
                    }
                }
            }
        }

        public double[] Compute(double[] inputs)
        {
            bool[] input = null;
            List<double> rtn = new List<double>();
            for (int i = 0; i < Newrons.Length; i++)
            {
                List<bool> tmpinput = new List<bool>();
                for (int j = 0; j < Newrons[i].Length; j++)
                {
                    if (i == 0)
                    {
                        var tmp = Newrons[i][j].OutputInputLayer(inputs[j]);
                        tmpinput.Add(tmp);
                    }
                    else if (i < Newrons.Length - 1)
                    {
                        var tmp = Newrons[i][j].GetOutput(input);
                        tmpinput.Add(tmp);
                    }
                    else
                    {
                        var tmp = Newrons[i][j].GetOutputOutputLayer(input);
                        rtn.Add(tmp);
                    }
                }
                input = tmpinput.ToArray();
            }
            return rtn.ToArray();
        }
    }

    class RandomUtil
    {
        static Random random = new Random();

        public static double GetValue()
        {
            return random.NextDouble();
        }

        public static void RandomizeDoubleArray(double[] Array)
        {
            for (int i = 0; i < Array.Length; i++)
                Array[i] = random.NextDouble();
        }
    }

    class Newron
    {
        public int inputCount = 0;
        public int outputCount = 0;
        public int layerNo = 0;
        public double[] waightInput;
        private double Threshould = 0;

        public Newron(int inputCount, int layerNo)
        {
            this.inputCount = inputCount;
            this.layerNo = layerNo;
            this.waightInput = new double[inputCount];
            RandomUtil.RandomizeDoubleArray(waightInput);
            Threshould = RandomUtil.GetValue();
        }

        public bool GetOutput(bool[] inputs)
        {
            double sum = 0;
            for (int i = 0; i < inputCount; i++)
            {
                sum += inputs[i] ? waightInput[i] : 0;
            }
            if (sum > Threshould)
                return true;
            return false;
        }

        public double GetOutputOutputLayer(bool[] inputs)
        {
            double sum = 0;
            for (int i = 0; i < inputCount; i++)
            {
                sum += inputs[i] ? waightInput[i] : 0;
            }
            return sum;
        }

        public bool OutputInputLayer(double input)
        {
            if (input > Threshould)
                return true;
            return false;
        }
    }
}
