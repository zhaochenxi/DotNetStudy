using System;

namespace 排序
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int[] a = { 2, 7, 1, 4, 5, 3, 6, 435, 6, };
            Console.WriteLine(a);
            Program.quick(ref a, 0, a.Length - 1);
            //Program.simple(ref a);
            Console.WriteLine(a);

        }
        static void simple(ref int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[i] > a[j])
                    {
                        int temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                    }
                }
            }
        }
        static void pop(ref int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                for (int j = a.Length - 1; j > i; j--)
                {
                    if (a[j] > a[j - 1])
                    {
                        int temp = a[j];
                        a[j] = a[j - 1];
                        a[j - 1] = temp;
                    }
                }
            }
        }


        static void quick(ref int[] a, int left, int right)
        {
            if (left >= right)
                return;
            int index = PartSort(ref a,left,right);
            quick(ref a, left, index-1);
            quick(ref a, index + 1, right);
        }
        static int PartSort(ref int[] array, int left, int right)
        {
            int key = right;
            while (left < right)
            {
                while (left < right && array[left] <= array[key])
                {
                    ++left;
                }
                while (left < right && array[right] >= array[key])
                {
                    --right;
                }
                swap(ref array[left], ref array[right]);
            }
            swap(ref array[left], ref array[key]);
            return left;
        }
        
        public static void heapsort(ref int[] arr)
        {
            //1.构建大顶堆
            for (int i = arr.Length / 2 - 1; i >= 0; i--)
            {
                //从第一个非叶子结点从下至上，从右至左调整结构
                adjustHeap(ref arr, i, arr.Length);
            }
            //2.调整堆结构+交换堆顶元素与末尾元素
            for (int j = arr.Length - 1; j > 0; j--)
            {
                swap(ref arr[0], ref arr[j]);//将堆顶元素与末尾元素进行交换
                adjustHeap(ref arr, 0, j);//重新对堆进行调整
            }

        }
        static void adjustHeap(ref int[] arr, int i, int length)
        {
            int temp = arr[i];//先取出当前元素i
            for (int k = i * 2 + 1; k < length; k = k * 2 + 1)
            {//从i结点的左子结点开始，也就是2i+1处开始
                if (k + 1 < length && arr[k] < arr[k + 1])
                {//如果左子结点小于右子结点，k指向右子结点
                    k++;
                }
                if (arr[k] > temp)
                {//如果子节点大于父节点，将子节点值赋给父节点（不用进行交换）
                    arr[i] = arr[k];
                    i = k;
                }
                else
                {
                    break;
                }
            }
            arr[i] = temp;//将temp值放到最终的位置
        }
        static void shellSort(ref int[] list)
        {
            int gap = list.Length / 2;

            while (1 <= gap)
            {
                // 把距离为 gap 的元素编为一个组，扫描所有组
                for (int i = gap; i < list.Length; i++)
                {
                    int j = 0;
                    int temp = list[i];

                    // 对距离为 gap 的元素组进行排序
                    for (j = i - gap; j >= 0 && temp < list[j]; j = j - gap)
                    {
                        list[j + gap] = list[j];
                    }
                    list[j + gap] = temp;
                }                
                gap = gap / 2; // 减小增量
            }
        }
        static void swap(ref int a,ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}
