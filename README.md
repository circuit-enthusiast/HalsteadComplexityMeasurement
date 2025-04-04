# Halstead Complexity Measurement

## Overview

Halstead Metrics Parameters according to [IBM](https://www.ibm.com/docs/en/devops-test-embedded/9.0.0?topic=metrics-halstead):
| Parameter | Description |
| - | - |
| n1 |	Number of distinct operators |
| n2 |	Number of distinct operands |
| N1 |	Number of operator instances |
| N2 |	Number of operand instances |

This repository aims to create a tool which will be used for the SOEN-345 (Software Testing) class of Winter 2025. The first step to get any halstead metrics will be to obtain the above parameter values from a given codebase.
Amongst the halstead metrics, the most crucial ones to measure for this poject are the *Difficulty* and the *Volume*.

Halstead Metrics and Formurla according to [IBM](https://www.ibm.com/docs/en/devops-test-embedded/9.0.0?topic=metrics-halstead):
| Metric |	Name |	Formula |
|-|-|-|
| n |	Vocabulary |	n_1 + n_2 |
| N	| Size |	N_1 + N_2 |
| **V**	| Volume |	N * log2 n |
| **D**	| Difficulty |	n_1/2 * N_2/n_2|
| E |	Effort |	V * D |
| B	| Errors |	V / 3000 |
| T	| Testing time |	E / k|

> In the above formulae, k is the stroud number, which has an arbitrary default value of 18.
[IBM](https://www.ibm.com/docs/en/devops-test-embedded/9.0.0?topic=metrics-halstead)

## Supported Languages

- Java (in-progress)
- C# (in-backlog)
- python (in-backlog)

## Usage

TODO
