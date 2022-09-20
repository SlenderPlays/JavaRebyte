package rebyte.helloworld;

import rebyte.helloworld.factories.HelloWorldStringFactory;
import rebyte.helloworld.Constants;

public class App {
    public static void main(String[] args) {
        // Please note, the factory pattern is not appropriate in this situation, this
        // is used just to make the code more verbose and complex for the sake of analysis by the JavaRebyte project.
        // Please, don't do this in production code.
        HelloWorldStringFactory factory = new HelloWorldStringFactory("Hello John");
        factory.password = "123";
        System.out.println(factory.getMessage());

        System.out.println(Constants.test);
        Constants.test = 2;
    }
}
