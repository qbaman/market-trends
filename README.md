What we added for the brief
Encapsulate What Varies

AppSettings holds the dataset path and row limit.
Change one place, the whole app follows.

Program to an Interface

IProductRepository abstracts how data is loaded.
Current impl: CsvProductRepository.

IProductSearch abstracts filtering logic.
Current impl: BrandSearchService.

Result: the form doesn’t know or care where data comes from or how search is done.

Favor Composition Over Inheritance

ProductsForm gets a repository and a search service in its constructor.
It composes behavior instead of subclassing to change behavior.

Clean Code (DRY, KISS, SoC)

Data access is in the repository.

Search logic is in the service.

UI only wires events, binds data, and draws charts.

Reused helpers (e.g., conversions) live in Calculations.

.gitignore excludes .vs/, bin/, obj/.
