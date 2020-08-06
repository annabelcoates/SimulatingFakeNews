import networkx as nx
import graph_functions as gf
import argparse
import matplotlib.pyplot as plt

#n = 6
parser= argparse.ArgumentParser(8)
parser.add_argument('--n', type=int)
parser.add_argument('--k', type= int)
args=parser.parse_args()
n=args.n
k=args.k
G = nx.watts_strogatz_graph(n,k,1)
gf.write_to_csv(G, "C:/Users/Anni/Documents/Uni/Computer "
                   "Science/Proj/small_world_graph.csv")
#nx.draw(G,with_labels=False,pos=nx.spectral_layout(G),
 #      node_size=10,linewidths=0.1,width=0.01,node_color=range(1000),
  #    cmap=plt.cm.Blues)

#plt.savefig("C:/Users/Anni/Documents/Uni/Computer "
#            "Science/Proj/Network Diagrams/MostRecentNxDrawing.png")
			
print("Done with n="+str(n))
plt.show()