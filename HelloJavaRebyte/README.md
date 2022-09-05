# HelloJavaRebyte
This project is a companion to JavaRebyte. The purpose is to create a jar file meant to be used by the test (JavaRebyte.Tests) framework.
Unfortunately, Visual Studio does not support java projects out of the box, and thus Visual Studio Code is reccomended when you wish to edit this project.

Custom scripts have been provided to handle the compilation process and testing.
For these to work, please create an environment variable with the name: `JAVA_REBYTE_JDK` witch will point to a JDK of your choosing.
(To be more precise, the value/path of this environment variable should be the path to the folder with the "java", "javac" and "jar" executables)
