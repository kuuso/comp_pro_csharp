using System;
using System.Collections;
using System.Collections.Generic;

static partial class Alg {
	
	public static List<int> EulerTourOrderIO(List<int>[] E, int root){
		// E: edge information (E[idx]: list of vertices from idx-node)
		// return: DFS order, list of index when in & out
		
		int n = E.Length;
		int[] cnt = new int[n];
		
		List<int> euler = new List<int>();
		
		var stk = new Stack<int>();
		stk.Push(root);
		while(stk.Count > 0){
			var now = stk.Pop();
			cnt[now]++;
			euler.Add(now);
			if(cnt[now] == 1){
				stk.Push(now);
				foreach(var nxt in E[now]){
					if(cnt[nxt] == 0){
						stk.Push(nxt);
					}
				}
			}
		}
		
		return euler;
	}
	
	public static List<int> EulerTourOrderWandering(List<int>[] E, int root){
		// E: edge information (E[idx]: list of vertices from idx-node)
		// return: DFS order, list of index when visited one by one
		// (possibly more frequent time than 2, only 1 time when node is leaf)
		
		int n = E.Length;
		int[] cnt = new int[n];
		int[] parent = new int[n];
		for(int i=0;i<n;i++) parent[i] = -1;
		
		List<int> euler = new List<int>();
		
		var stk = new Stack<int>();
		stk.Push(root);
		while(stk.Count > 0){
			var now = stk.Pop();
			cnt[now]++;
			euler.Add(now);
			if(cnt[now] == 1){
				if(parent[now] != -1) stk.Push(parent[now]);
				foreach(var nxt in E[now]){
					if(cnt[nxt] == 0){
						parent[nxt] = now;
						stk.Push(nxt);
					}
				}
			}
		}
		
		return euler;
	}
	
}


class Test {
	static void Main(){
		
		int N = 12;
		int[] parent = new int[]{
			-1, 0, 1, 2, 3, 2, 5, 0, 7, 8, 0, 10
		};
		// 0 - 1 - 2 - 3 - 4
		//           - 5 - 6
		//   - 7 - 8 - 9
		//   - 10 - 11
		
		List<int>[] E = new List<int>[N];
		for(int i=0;i<N;i++) E[i] = new List<int>();
		for(int i=1;i<N;i++){
			E[i].Add(parent[i]);
			E[parent[i]].Add(i);
		}
		
		int root = 0;
		var eulerIO = Alg.EulerTourOrderIO(E, root);
		Console.WriteLine("eulerIO: \n{0}",String.Join(" ", eulerIO));
		// in and out only
		//   [0, 10, 11, 11, 10, 7, 8, 9, 9, 8, 7, 1, 2, 5, 6, 6, 5, 3, 4, 4, 3, 2, 1, 0]
		
		var eulerWandering = Alg.EulerTourOrderWandering(E, root);
		Console.WriteLine("eulerWandering: \n{0}",String.Join(" ", eulerWandering));
		// wandering 1 by 1
		//   [0, 10, 11, 10, 0, 7, 8, 9, 8, 7, 0, 1, 2, 5, 6, 5, 2, 3, 4, 3, 2, 1, 0]		
	}
}
