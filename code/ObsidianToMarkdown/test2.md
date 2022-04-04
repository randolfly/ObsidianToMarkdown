# ESN 原理和推导

> - tags: #拟合 #ESN #机器学习

## 定义

对于这样一个任务，给定一段信号：$u(0), u(1), \cdots u(N_t-1)$，及其对应的目标值：$v(1), v(2), \cdots v(N_t)$，给出一个黑箱模型M，使得我们可以预测得到：$v(N_t+1), v(N_t+2), \cdots$

ESN(回声状态网络)可以用来解决这样的问题。其具有如下的特点：

- 思想
  - 使用大规模随机稀疏网络（储备池）作为信息处理媒介，将输入信号从低维的输入空间映射到高维的状态空间，在高维的状态空间采用线性回归方法对网络的部分连接权进行训练，而其他连接权随机产生，并在网络训练过程中保持不变。==递归神经网络输出连接权改变迅速，而内部连接权则以高度耦合的方式缓慢改变。也就是说，如果递归神经网络内部连接权选择合适，在对网络进行训练时可以忽略内部连接权的改变。==
- 优势
  - 简化了网络的训练过程，解决了传统递归神经网络结构难以确定、训练算法过于复杂的问题，同时也可以克服递归网络存在的记忆消减等问题

## 实现

![](attachments/20210914_ESN.jpeg)

上面是ESN结构的图示，中间的大圆圈称为“储备池”，有以下特点：

- 包含数目较多的神经元（与经典神经网络相比）
- 神经元之间的连接关系随机产生
- 神经元之间的连接具有稀疏性

其中满足：
$$
u\in R^{M*1}, W_{IR}\in R^{N*M}, W_{res}\in R^{N*N}, r\in R^{N*1}, W_{RO}\in R^{L*N}, v\in R^{L*1},
$$
上面的两个参数矩阵$W_{IR}\in R^{N*M}, W_{res}\in R^{N*N}$都是事先给定的数值，因此训练的过程中需要训练的是$W_{RO}\in R^{L*N}$

**计算过程**

1. 从输入到储备池(reservoir)的运算：$W_{IR}*u(t)$
2. 储备池中r(t)的更新：$r(t+\Delta t)=f[W_{res}*r(t)+W_{IR}*u(t)]$
3. 从储备池到输出：$W_{RO}*r(t)$
4. 损失函数：$L=\sum_{t=d+1}^{N_t}|v(t)-W_{RO}*r(t)|^2$
5. 使损失函数最小化

**推导过程**
$$
\begin{aligned}
L &= |W_{RO}r-v|^2=(r^TW_{RO}^T-v^T)(W_{RO}r-v)\\
  &= r^TW_{RO}^TW_{RO}r-2r^TW_{RO}^Tv+v^Tv\\
\end{aligned}
$$
进一步损失函数对$W_{RO}$求偏导：
$$
\frac{\partial{L}}{\partial W_{RO}} = \frac{\partial r^TW_{RO}^TW_{RO}r}{\partial W_{RO}}-2r^Tv
$$
其中
$$
\frac{\partial r^TW_{RO}^TW_{RO}r}{\partial W_{RO}} = \frac{\partial r^TW_{RO}^T}{\partial W_{RO}}W_{RO}r+\frac{\partial r^TW_{RO}^T}{\partial W_{RO}}W_{RO}r = 2r^TW_{RO}r
$$
参考了[矩阵求导](../数学/线性代数/矩阵求导.md)

因此需要使得 ：
$$
\frac{\partial{L}}{\partial W_{RO}} = 2r^TW_{RO}r-2r^Tv=0
$$
从而得到了：
$$
W_{RO}r=v
$$
进而有：
$$
W_{RO}rr^T=vr^T
$$
从而：
$$
W_{RO}=vr^T(rr^T)^{-1}
$$
给损失函数增加一个正则化项得到：
$$
L = |W_{RO}r-v|^2-\eta |W_{RO}|^2
$$
因此最终的更新方法为：
$$
W_{RO}=vr^T(rr^T+\eta I)^{-1}
$$
**ESN的预测步骤**

在$W_{RO}$确定之后，输出可以表示为：
$$
\begin{aligned}
u(t) &= W_{RO}*r(t)\\
r(t+\Delta t) &= f[W_{res}*r(t)+W_{IR}*u(t)]\\
u(t+\Delta t) &= W_{RO}*r(t+\Delta t)
\end{aligned}
$$
预测的时候不会再给单独的输入了，而是会将输出作为输入进行递推计算。

注意：

- 一般$W_{IR}$各元素会初始化为$[-\alpha,\alpha]$之间的均匀分布
- 每个输入$u(t)$都会和$N/M$个库中的节点相连，输入个数为M时，库中有N个节点，即：$u\in R^{M*1}, r\in R^{N*1}$
- $W_{res}$一般是一个大型，稀疏，有向或无向的随机网络，平均度为k，谱半径$\rho(W_{res})$是$W_{res}$最大的特征值
- $W_{res}$初始化为一个稀疏矩阵
- 输入特征可以是多维的

## 计算实例

## 参考

- [回声状态网络（ESN）的公式推导及代码实现_comli_cn的博客-CSDN博客_esn网络](https://blog.csdn.net/comli_cn/article/details/109394553)
- [回声状态网络(ESN)教程_cassiePython的专栏-CSDN博客](https://blog.csdn.net/cassiePython/article/details/80389394)
