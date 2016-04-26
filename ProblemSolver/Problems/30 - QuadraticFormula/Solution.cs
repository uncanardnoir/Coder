public class MySolution {
    public static double QuadraticFormula(string input) {
        string[] inputs = input.Split(';');
        int a = int.Parse(inputs[0]);
        int b = int.Parse(inputs[1]);
        int c = int.Parse(inputs[2]);
        double root = System.Math.Sqrt(b * b - 4 * a * c);
        return (-b + root) / (2 * a);
    }
}