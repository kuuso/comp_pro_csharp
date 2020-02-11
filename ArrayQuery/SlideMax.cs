using System;
using System.Collections;
using System.Collections.Generic;

static class Alg{
	public static int[] SlideMaxIndex<T>(T[] a, int K, Func<T, T, int> cmp, bool rightPreferred = true){
		// arr:List[T], cmp: T,T -> [-1,0,1]
		// returns idx of max in [i - K + 1, i] (closed-closed) for each i in [0, N)
		int N = a.Length;
		int[] maxIdx = new int[N];
		
		var deq = new Deque<int>();
		for(int i=0;i<N;i++){
			while(deq.Count > 0 && deq.First <= i - K) deq.RemoveFirst();
			while(deq.Count > 0 && cmp(a[deq.Last], a[i]) < 0) deq.RemoveLast();
			if(rightPreferred){
				while(deq.Count > 0 && cmp(a[deq.Last], a[i]) <= 0) deq.RemoveLast();
			}
			deq.AddLast(i);
			maxIdx[i] = deq.First;
		}
		
		return maxIdx;
	}
	
	public static int[] Naive<T>(T[] a, int K, Func<T, T, int> cmp, bool rightPreferred = true){
		
		int N = a.Length;
		int[] maxIdx = new int[N];
		
		for(int i=0;i<N;i++){
			T ma = a[i];
			int idx = i;
			for(int j = i - K + 1; j <= i; j++){
				if(j < 0) continue;
				if(cmp(ma, a[j]) < 0){
					ma = a[j];
					idx = j;
				}
				if(cmp(ma, a[j]) == 0){
					if(rightPreferred){
						idx = Math.Max(idx, j);
					} else {
						idx = Math.Min(idx, j);
					}
				}
			}
			maxIdx[i] = idx;
		}
		return maxIdx;
	}
	
}

class Deque<T> :IEnumerable<T>{
	T[] arr;
	int head, tail;
	int count;
	int capacity;
	
	public int Capacity{
		get{ return capacity;}
	}
	public int Count{
		get{ return count;}
	}
	
	public T First {
		get { 
			if(count == 0) throw new Exception("empty");
			return arr[head];
		}
	}
	public T Last {
		get { 
			if(count == 0) throw new Exception("empty");
			return arr[tail]; 
		}
	}
	
	public T this [int pos]{
		get{
			if(pos >= count) throw new ArgumentOutOfRangeException("out of range");
			return arr[(head + pos) % arr.Length];
		}
	}
	
	public IEnumerator<T> GetEnumerator(){
		for(int i=0;i<count;i++){
			yield return arr[(head + i) % arr.Length];
		}
	}
	
	IEnumerator IEnumerable.GetEnumerator() {
		return this.GetEnumerator();
	}
		
	public void AddFirst(T elem){
		if(count == capacity) {
			Twice();
		}
		
		head = count == 0 ? head : (head - 1);
		if(head < 0) head = arr.Length - 1;
		arr[head] = elem;
		count++;
		
	}
	
	public void RemoveFirst(){
		if(count == 0) throw new Exception("empty");
		arr[head] = default(T);
		count--;
		head = count == 0 ? head : head + 1; if(head == arr.Length) head = 0;
	}
	
	public void AddLast(T elem){
		if(count == capacity) {
			Twice();
		}
		
		tail = count == 0 ? tail : (tail + 1);
		if(tail >= arr.Length) tail = 0;
		arr[tail] = elem;
		count++;
		
	}
	
	public void RemoveLast(){
		if(count == 0) throw new Exception("empty");
		arr[tail] = default(T);
		count--;
		tail = count == 0 ? tail : tail - 1; if(tail < 0) tail = arr.Length - 1;
	}
	
	void Twice(){
		
		T[] nxt = new T[arr.Length * 2];
		int nhead = arr.Length / 2;
		for(int i = 0; i < count; i++) nxt[nhead + i] = arr[(head + i) % arr.Length];
		head = nhead;
		tail = head + count - 1;
		arr = nxt; 
		capacity = arr.Length;
	}
	
	public Deque() {
		Init(256);
	}
	
	public Deque(int cap_){
		Init(cap_);
	}
	
	void Init(int cap){
		if(cap < 0) cap = 256;
		arr = new T[cap];
		capacity = cap;
		head = cap / 2;
		tail = cap / 2;
		count = 0;
	}

}


class Test{
	static void Main(){
		int[] a = new int[]{6,5,4,4,3,2,3,4,4,5,7,2,2};
		bool[] yn = new bool[]{ true, false};
		int[] ks = new int[]{1, 3, 6};
		Func<int, int, int> cmp = (x, y) => x.CompareTo(y);
		foreach(var rightPreferred in yn){
			foreach(var K in ks){
				var res = Alg.SlideMaxIndex(a, K, cmp, rightPreferred);
				var gt = Alg.Naive(a, K, cmp, rightPreferred);
				Console.WriteLine("K:{0}, rightPreferred: {1}, assert: {2}, res:{3}, gt:{4}",
					K, rightPreferred, Assert(res, gt), String.Join(" ", res), String.Join(" ",gt)
				);
			}
		}
	}
	static bool Assert(int[] a, int[] b){
		if(a.Length != b.Length) return false;
		for(int i=0;i<a.Length;i++) if(a[i] != b[i]) return false;
		return true;
	}
}
