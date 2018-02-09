using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SearchText
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            BenchMark(5);
            
            BenchMark(10);

            BenchMark(20);

            BenchMark(30);

            BenchMark(40);

            BenchMark(60);

            BenchMark(120);
        }

        public static void BenchMark(int len,int count=1000)
        {
            Console.WriteLine($"BenchMark len:{len} count:{count} start");
            Stopwatch sw = new Stopwatch();

            sw.Start();
            string str2 = GetString(len);
            //str2 = str2.Substring(0,3)+str2.Substring(0, 3)+str2;
            string str22=str2.Substring(0,str2.Length>10?10:str2.Length);

            string[] arrStr10000 = new string[1000];
            for (int i = 0; i < arrStr10000.Length; i++)
            {

                arrStr10000[i] = GetString(i% 30)+(i%100==0?str2:"")+ GetString(len)+ (i % 20 == 0 ? str2 : "");
            }

            sw.Stop();
            Console.WriteLine($"arrStr10000 build string[] ms:{sw.ElapsedMilliseconds}");

            sw.Restart();
            int n = 0;
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (IndexOf(str, str2) >= 0)
                    {
                        n++;
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"arrStr10000 IndexOf ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            var next = GetNextV2(str2);
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (KMPIndexOf(str, str2, next) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"arrStr10000 KMPIndexOf ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (str.IndexOf(str2) >= 0)
                    {
                        n++;
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"arrStr10000 C# IndexOf ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            var next2 = GetNext(str2);
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (search(str,str2, next2) >= 0)
                    {
                        n++;
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"arrStr10000 Search ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            var next4 = nextV4(str2);
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (Index_KMP(str, str2, next4) >= 0)
                    {
                        n++;
                    }
                }
            }
            sw.Stop();

            Console.WriteLine($"arrStr10000 Index_KMP ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (BMIndex(str, str2) >= 0)
                    {
                        n++;
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 BMIndex 1 ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (BMIndex(str, str2,2) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 BMIndex 2 ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (BMIndex(str, str2,3) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 BMIndex 3 ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (SundayIndex(str, str2) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 SundayIndex ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");

            sw.Restart();

            //string[] keywords = new string[] { "ab", "abc", "bcd", "abf", "bca" };
            //string strText = "hello ab,this is abcbcdabf abbcd ab abf abca";

            string[] keywords = new string[] { str2 };
            var ac = new AhoCorasick();
            ac.Build(keywords);

            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (ac.SearchFirst(str,0).Index>=0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 AC_SearchFirst ms:{sw.ElapsedMilliseconds} n:{n} searchText:{str22}");


            Console.WriteLine($"===============arrStr10000 find 3 words");

            sw.Restart();

            //string[] keywords = new string[] { "ab", "abc", "bcd", "abf", "bca" };
            //string strText = "hello ab,this is abcbcdabf abbcd ab abf abca";

            var keywords2 = new string[] { str2, str2 + "a", str2 + "b" };
            ac = new AhoCorasick();
            ac.Build(keywords2);

            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (ac.SearchFirst(str, 0).Index >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 AC_SearchFirst ms:{sw.ElapsedMilliseconds} n:{n} words:{keywords2.Length}");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                n = 0;
                foreach (var str in arrStr10000)
                {
                    if (SundayIndex(str, keywords2[0]) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }

                    if (SundayIndex(str, keywords2[1]) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }

                    if (SundayIndex(str, keywords2[2]) >= 0)
                    {
                        n++;
                        //Console.WriteLine(str);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"arrStr10000 SundayIndex ms:{sw.ElapsedMilliseconds} n:{n} words:{keywords2.Length}");

            sw.Restart();

            Console.WriteLine();
            Console.WriteLine($"BenchMark len:{len} count:{count} end");
        }

        public static string GetString(int n)
        {
            if(n<=0)
            {
                return string.Empty;
            }

            Random ran = new Random((int)DateTime.Now.ToBinary());
            char[] arr = new char[n];

            for(int i=0;i<n;i++)
            {
                arr[i]=(char)ran.Next(97, 122);
            }
            return new string(arr);
        }

        public static int search(String original, String find, int[] next)
        {
            int j = 0;
            for (int i = 0; i < original.Length; i++)
            {
                while (j > 0 && original[i] != find[j])
                    j = next[j];

                if (original[i] == find[j])
                    j++;

                if (j == find.Length)
                {
                    //System.out.println("find at position " + (i - j));
                    //System.out.println(original.subSequence(i - j + 1, i + 1));
                    j = next[j];
                }
            }
            return j;
        }

        public static int[] GetNext(String b)
        {
            int len = b.Length;
            int j = 0;

            int[] next = new int[len + 1]; //next表示长度为i的字符串前缀和后缀的最长公共部分，从1开始  
            next[0] = next[1] = 0;

            for (int i = 1; i < len; i++)//i表示字符串的下标，从0开始  
            {//j在每次循环开始都表示next[i]的值，同时也表示需要比较的下一个位置  
                while (j > 0 && b[i] != b[j])
                    j = next[j];

                if (b[i] == b[j])
                    j++;

                next[i + 1] = j;
            }

            return next;
        }

        public static int KMPIndexOf(string str,string b,int[] next)
        {
            int len = str.Length;
            int bLen = b.Length;
            int index = -1;
            int partVal = 0;

            for(int i=0;i<len;)
            {
                index = i;

                if (len - i < bLen)
                {
                    index = -1;
                    break;
                }

                for (int j = 0; j < bLen; j++)
                {
                    if(str[i+j]!=b[j])
                    {
                        index = -1;
                        break;
                    }
                    partVal=j;
                }

                if (index != -1)
                    break;

                if (partVal > 0)
                {
                    i += partVal-next[partVal];
                    partVal = 0;
                }
                else
                {
                    i++;
                }
            }


            return index;
        }

        public static int IndexOf(string str, string b)
        {
            int len = str.Length;
            int bLen = b.Length;
            int index = -1;

            for (int i = 0; i < len; i++)
            {
                index = i;
                if (len - i < bLen)
                {
                    index = -1;
                    break;
                }

                for (int j = 0; j < bLen; j++)
                {
                    if (str[i + j] != b[j])
                    {
                        index = -1;
                        break;
                    }

                }

                if (index != -1)
                    break;

            }
            
            return index;
        }

        public static int[] GetNextV2(string b)
        {
            //abcdabd
            int len = b.Length;
            int j = 0;

            int[] next = new int[len];
            next[0] = 0;
            
            for(var i=1; i<len;i++)
            {
                while(j>0&&b[i]!=b[j])
                {
                    j--;
                }

                if (b[i] == b[j])
                    j++;

                next[i] = j;


            }
            

            return next;
        }

        public static int Index_KMP(String S, String P,int[] next)
        {
            int i = 0, j = 0;

            while (i < S.Length && j < P.Length)
            {
                if (j == -1 || S[i] == P[j])
                {        //    如果j = -1，或者当前字符匹配成功（即S[i] == P[j]），都令i++，j++. 注意：这里判断顺序不能调换！ 
                    i++;
                    j++;
                }
                else
                {
                    //    如果j != -1，且当前字符匹配失败（即S[i] != P[j]），则令 i 不变，j = next[j]      
                    //    next[j]即为j所对应的next值，效果为进行回溯       
                    //i += j - next[j];
                    j = next[j];
                    
                }
            }

            if (j == P.Length)
                return i - j;
            else
                return -1;
        }

        public static int[] nextV4(String p)
        {
            int[] next = new int[p.Length];
            int k = -1, j = 0;
            next[0] = -1;        //    初值为-1    

            while (j < p.Length - 1)
            {
                //    p[k]表示字符串的前缀，p[j]表示字符串的后缀
                if (k == -1 || p[k] == p[j])
                {  // 判断的先后顺序不能调换
                    k++;
                    j++;
                    //    后面即是求next[j+1]的过程
                    if (p[k] ==p[j])             //  此处等价于if(p[j] == p[ next[j] ])
                                                                //    因为不能出现p[j] = p[ next[j] ]，所以当出现时需要继续递归，k = next[k] = next[next[k]]
                        next[j] = next[k];                    //  此处等价于next[j] = next[ next[j] ]
                    else
                        next[j] = k;
                }
                else
                {
                    k = next[k];
                }
            }

            return next;
        }

        private static int BMIndexByChar(string str2,char c)
        {
            return str2.LastIndexOf(c);
        }

        private static int BMIndexByCharV2(string str2, char c)
        {
            for(var i=str2.Length-1; i>=0;i--)
            {
                if (str2[i] == c)
                    return i;
            }
            return -1;
        }

        public static int BMIndex(string str,string str2,int v=0)
        {
            if(str.Length<str2.Length)
                return -1;

            int i = str2.Length - 1;
            int j = 0;
            int k = str2.Length - 1;

            char c;

            int ii = 0;
            int ii2 =0;

            // bool isOver = false;

            int[] next=null;

            while (i < str.Length)
            {
                c = str[i];
                if (c == str2[k])
                {
                    ii = i - 1;
                    ii2 = i - 1 - k;
                    if(ii2 <-1)
                    {
                        //Console.WriteLine($"i:{i},k:{k},ii:{ii},ii2:{ii2},str:{str},str2:{str2}");
                        //ii2=-1;
                        break;
                    }

                    for (ii = i - 1; ii > ii2; ii--)
                    {
                        c = str[ii];
                        if (c != str2[--k])
                            break;
                    }

                    if (k == 0)
                    {
                        return ii + 1;
                    }

                }

                if (v==2)
                {
                    j = BMIndexByCharV2(str2, c);
                }
                else if(v==3)
                {
                    if(next== null)
                    {
                        next = new int[str2.Length];
                        for(int nn=0;nn<str2.Length;nn++)
                        {
                            next[nn]=str2[nn];
                        }
                    }

                    j=-1;
                    for(var nn=str2.Length-1;nn>=0;nn--)
                    {
                        if(next[nn]==c)
                        {
                            j=nn;
                            break;
                        }
                    }
                    
                }
                else
                {
                    j = BMIndexByChar(str2, c);
                }

                
                i += k - j;

                // if (!isOver&&i>str.Length-1)
                // {
                //     isOver = true;
                //     i = str.Length - 1;
                // }

                k = str2.Length - 1;
            }


            return -1;
        }

        public static int SundayIndex(string str,string str2)
        {
            int i=0;
            int i2=0;
            int i3=0;
            int i4 = 0;
            int index = 0;

            int k=str2.Length;
            
            int n=0;
            int m=0;

            while(i<=str.Length-str2.Length)
            {
                if(str[i]==str2[0])
                {
                    n=k-1;
                    i2=i+1;
                    i3=i+k;
                    i4 = 0;
                    try
                    {
                        for (int ii = i2; ii < i3; ii++)
                        {
                            if (str[ii] != str2[++i4])
                            {
                                break;
                            }

                            n--;
                            if (n == 0)
                                return i;
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"n:{n},i2:{i2},i3:{i3} str:{str},str2:{str2} ");
                        return 0;
                    }
                }

                index = i + str2.Length;
                if (index >=str.Length)
                    break;

                m = str2.LastIndexOf(str[index]);
                if (m > 0)
                {
                    //i =index- m;
                    i +=k- m;
                }
                else
                {
                    i = index;
                }
            }

            if (str.Contains(str2))
                Console.WriteLine($"SundayIndex str:{str},str2:{str2}");

            return -1;
        }
    }
}
