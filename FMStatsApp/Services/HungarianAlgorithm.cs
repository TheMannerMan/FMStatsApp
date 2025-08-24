namespace FMStatsApp.Services
{
	public class HungarianAlgorithm
	{
		public static (int[] assignment, double totalCost) Solve(double[,] costMatrix)
		{
			int n = costMatrix.GetLength(0);
			int m = costMatrix.GetLength(1);
			
			if (n > m)
			{
				// Lägg till dummy kolumner
				var expandedMatrix = new double[n, n];
				for (int i = 0; i < n; i++)
				{
					for (int j = 0; j < n; j++)
					{
						expandedMatrix[i, j] = j < m ? costMatrix[i, j] : double.MaxValue;
					}
				}
				var result = SolveSquare(expandedMatrix);
				return (result.assignment, result.totalCost);
			}
			else if (m > n)
			{
				// Lägg till dummy rader
				var expandedMatrix = new double[m, m];
				for (int i = 0; i < m; i++)
				{
					for (int j = 0; j < m; j++)
					{
						expandedMatrix[i, j] = i < n ? costMatrix[i, j] : double.MaxValue;
					}
				}
				var result = SolveSquare(expandedMatrix);
				// Returnera bara de relevanta tilldelningarna
				var trimmedAssignment = new int[n];
				Array.Copy(result.assignment, trimmedAssignment, n);
				return (trimmedAssignment, result.totalCost);
			}
			else
			{
				return SolveSquare(costMatrix);
			}
		}
		
		private static (int[] assignment, double totalCost) SolveSquare(double[,] costMatrix)
		{
			int n = costMatrix.GetLength(0);
			var assignment = new int[n];
			var u = new double[n + 1];
			var v = new double[n + 1];
			var p = new int[n + 1];
			var way = new int[n + 1];
			
			for (int i = 1; i <= n; ++i)
			{
				p[0] = i;
				int j0 = 0;
				var minv = new double[n + 1];
				var used = new bool[n + 1];
				
				for (int j = 0; j <= n; ++j)
				{
					minv[j] = double.MaxValue;
					used[j] = false;
				}
				
				do
				{
					used[j0] = true;
					int i0 = p[j0];
					double delta = double.MaxValue;
					int j1 = 0;
					
					for (int j = 1; j <= n; ++j)
					{
						if (!used[j])
						{
							double cur = costMatrix[i0 - 1, j - 1] - u[i0] - v[j];
							if (cur < minv[j])
							{
								minv[j] = cur;
								way[j] = j0;
							}
							if (minv[j] < delta)
							{
								delta = minv[j];
								j1 = j;
							}
						}
					}
					
					for (int j = 0; j <= n; ++j)
					{
						if (used[j])
						{
							u[p[j]] += delta;
							v[j] -= delta;
						}
						else
						{
							minv[j] -= delta;
						}
					}
					
					j0 = j1;
				} while (p[j0] != 0);
				
				do
				{
					int j1 = way[j0];
					p[j0] = p[j1];
					j0 = j1;
				} while (j0 != 0);
			}
			
			double totalCost = 0;
			for (int j = 1; j <= n; ++j)
			{
				if (p[j] != 0)
				{
					assignment[p[j] - 1] = j - 1;
					totalCost += costMatrix[p[j] - 1, j - 1];
				}
			}
			
			return (assignment, totalCost);
		}
	}
}