import networkx as nx
import graph_functions as gf
import argparse

#n = 6
parser= argparse.ArgumentParser(description="Parse n")
parser.add_argument('--n', type=int)
args=parser.parse_args()
n=args.n
G = nx.wheel_graph(n)
gf.write_to_csv(G, "C:/Users/Anni/Documents/Uni/Computer "
                   "Science/Proj/wheel_graph2.csv")
print("Done with n="+str(n))