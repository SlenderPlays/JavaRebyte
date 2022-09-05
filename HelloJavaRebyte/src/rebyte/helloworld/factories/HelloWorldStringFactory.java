package rebyte.helloworld.factories;

public class HelloWorldStringFactory {
    public String password = "hunter2";

    private String message;

    public HelloWorldStringFactory()
    {
        setMessage("Hello World");
    }
    public HelloWorldStringFactory(String message)
    {
        setMessage(message);
    }


    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }
}
