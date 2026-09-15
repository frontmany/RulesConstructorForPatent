namespace PIS_6sem.Catalogs
{
    // Вид условия профиля: название свойства и значения, из которых можно выбирать.
    public record ProfilePropertyKind(string Name, IReadOnlyList<string> Values);
}
