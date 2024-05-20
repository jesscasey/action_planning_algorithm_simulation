# Using Reinforcement Learning to Automate User Feedback Testing

This project was created as part of Jessica Casey's dissertation (module code
CSC3094) at Newcastle University.

## Overview

This project aims to create a solution that automates user feedback testing by
training a reinforcement learning agent to make decisions in a turn-based
combat game based on its perception of any hints provided. The agent's
performance under a given hint system should be compared to that when no hints
are provided to assess the effectiveness of a hint system.

### Scenes

This project contains three versions of a turn-based combat game. They are
almost identical, with one key difference. One scene displays no hints, while
the remaining scenes utilise different methods of offering hints.

### Training the agent

The agent was trained using [Unity's ML-Agents Toolkit](https://github.com/Unity-Technologies/ml-agents),
an open-source library that enables training intelligent agents via 
communication between the Unity game engine and a Python API.