# AssigningTaskToAndGettingResponseFromWorkerThread

- This repo aims to simulate how main thread assign tasks to worker thread without blocking main thread.
- Main thread continue to perform it's tasks, and worker thread performs tasks assigned to it.
- Once the worker thread is done with its tasks it return the result to the main thread.
- After getting result returned from worker thread, the main thread completes tasks related to results and then do other tasks.
