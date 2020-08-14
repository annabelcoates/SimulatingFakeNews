import pandas as pd
import matplotlib.pyplot as plt
import numpy as np

for i in range(300,310,1):
    nShared = [0] * 1000
    nShared_df=np.array(pd.read_csv("C:/Users/Anni/Documents/Uni/Computer "
                         "Science/Proj/CSVs "
                       "and text files/nShared"+str(i)+".csv"))
    missing=1000-len(nShared_df)
    nShared[missing:1000]=nShared_df
    plt.plot(nShared,label=str(i-300))
plt.legend()
plt.show()