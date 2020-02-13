using System;
using System.Collections;
using System.Collections.Generic;

static partial class Alg {
	public static int[] FirstExceeder<T>(T[] arr_, Func<T, T, int> cmp_, bool strictExceed = true, bool left = false) {
		int N = arr_.Length;
		Func<T, T, int> cmp = cmp_;
		if(!strictExceed) {
			// encount equality: regard as exceeded
			cmp = (x, y) => cmp_(x, y) == 0 ? -1 : cmp_(x, y);
		}
		T[] arr = new T[N];
		for(int i = 0; i < N; i++) arr[i] = arr_[i];
		if(left) {
			Array.Reverse(arr);
		}

		int[] exceeder = new int[N];
		var stk = new Stack<int>();
		for(int i = 0; i < N; i++){
			while(stk.Count > 0) {
				var top = stk.Peek();
				if(cmp(arr[top], arr[i]) < 0) {
					exceeder[top] = i;
					stk.Pop();
					continue;
				}
				break;
			}
			stk.Push(i);
		}
		while(stk.Count > 0) exceeder[stk.Pop()] = N;

		if(left) {
			Array.Reverse(exceeder);
			for(int i = 0; i < N; i++) exceeder[i] = N - 1 - exceeder[i];
		}
		return exceeder;
	}

}

static partial class Checker {
	public static int[] Naive_FirstExceeder<T>(T[] arr, Func<T, T, int> cmp, bool strictExceed = true, bool left = false) {
		int N = arr.Length;
		int[] exceeder = new int[N];
		if(!left) {
			for(int i = 0; i < N; i++) exceeder[i] = N;
			for(int i = 0; i < N; i++) {
				for (int j = i + 1; j < N; j++) {
					if(strictExceed && cmp(arr[i], arr[j]) < 0) {
						exceeder[i] = j;
						break;
					}
					if(!strictExceed && cmp(arr[i], arr[j]) <= 0) {
						exceeder[i] = j;
						break;
					}
				}
			}
		} else {
			for(int i = 0; i < N; i++) exceeder[i] = -1;
			for(int i = 0; i < N; i++) {
				for (int j = i - 1; j >= 0; j--) {
					if(strictExceed && cmp(arr[i], arr[j]) < 0) {
						exceeder[i] = j;
						break;
					}
					if(!strictExceed && cmp(arr[i], arr[j]) <= 0) {
						exceeder[i] = j;
						break;
					}
				}
			}
		}
		return exceeder;
	}
}



class Test {
	static void Main(){
		int[] a = new int[]{6,5,4,4,3,2,3,4,4,5,7,2,2};
		bool[] yn = new bool[]{true, false};
		foreach(var left in yn) {
			foreach(var strictExceed in yn) {
				var res = Alg.FirstExceeder(a, (x, y) => x.CompareTo(y), strictExceed, left);
				var gt = Checker.Naive_FirstExceeder(a, (x, y) => x.CompareTo(y), strictExceed, left);
				Console.WriteLine("strictExceed: {0}, left: {1}, assert: {2}",strictExceed, left, Assert(res, gt));
				Console.WriteLine("res: {0}", String.Join(" ", res));
				Console.WriteLine("gt : {0}", String.Join(" ", gt));
			}
		}
	}
	static bool Assert(int[] a, int[] b){
		if(a.Length != b.Length) return false;
		for(int i=0;i<a.Length;i++) if(a[i] != b[i]) return false;
		return true;
	}
}
