using Ardalis.Specification;
using Core.DTOs.Filter;
using Core.Entities.Product;

namespace Core.Specification
{
    public class ProductSpecification
    {
        public class GetProductByCategoryId : Specification<ProductEntity>
        {
            public GetProductByCategoryId(int id)
            {
                Query
                    .Include(p => p.Images)
                    .Include(p => p.Color)
                    .Include(p => p.Season)
                    .Include(p => p.Material)
                    .Where(p => p.CategoryId == id);
            }
        }
        public class ProductById : Specification<ProductEntity>
        {
            public ProductById(int id)
            {
                Query
                    .Where(p => p.Id == id)
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Season)
                    .Include(p => p.Material)
                    .Include(p => p.Storages);
            }
        }
        public class ProductByPage : Specification<ProductEntity>
        {
            public ProductByPage(int page, string search)
            {
                if (page < 1)
                {
                    page = 1;
                }
                int pageSize = 10;

                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Replace("_", " ");
                    Query.Where(p => p.Article.ToLower().Contains(search.ToLower()));
                }
                Query.OrderBy(p => p.Id)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Include(p => p.Images)
                     .Include(p => p.Category)
                     .Include(p => p.Color)
                     .Include(p => p.Season)
                     .Include(p => p.Material)
                     .Include(p => p.Storages.OrderBy(s => s.SortOrder));
            }
        }

        public class ProductQuantityByPage : Specification<ProductEntity>
        {
            public ProductQuantityByPage(string search)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Replace("_", " ");
                    Query.Where(p => p.Article.ToLower().Contains(search.ToLower()));
                }
                Query.OrderBy(p => p.Id);
            }
        }

        public class FilterProducts : Specification<ProductEntity>
        {
            public FilterProducts(FilterDTO? filterDTO)
            {
                List<string> Colors = filterDTO.Color ?? new List<string>();
                List<string> Materials = filterDTO.Material ?? new List<string>();
                List<string> Sizes = filterDTO.Size ?? new List<string>();
                List<string> Season = filterDTO.Season ?? new List<string>();
                int page = filterDTO.Page < 1 ? 1 : filterDTO.Page;
                string sort = filterDTO.Sort;
                string search = filterDTO.Search;
                int pageSize = filterDTO.ItemsPerPage;

                Query
                     .Include(p => p.Images)
                     .Include(p => p.Storages.OrderBy(p => p.SortOrder))
                     .Include(p => p.Color)
                     .Include(p => p.Season)
                     .Include(p => p.Material)
                     .Include(p => p.Category)
                     .ThenInclude(p => p.SubCategory)
                     .ThenInclude(p => p.MainCategory)
                     .Where(p => p.Storages != null && p.Storages.Any(s => s.inStock));

                if (!string.IsNullOrEmpty(filterDTO.MainCategory))
                {
                    Query.Where(p => p.Category.SubCategory.MainCategory.URLName == filterDTO.MainCategory);
                }
                if (filterDTO.urlName != null)
                {
                    Query.Where(p => filterDTO.urlName.Contains(p.Category.URLName));
                }
                if (filterDTO.subName != null)
                {
                    Query.Where(p => filterDTO.subName.Contains(p.Category.SubCategory.URLName));
                }
                if (Colors.Any())
                {
                    Query.Where(p => Colors.Contains(p.Color.Value.ToLower()));
                }
                if (Materials.Any())
                {
                    Query.Where(p => Materials.Contains(p.Material.Value.ToLower()));
                }
                if (Sizes.Any())
                {
                    Query.Where(p => p.Storages != null && p.Storages.Any(s => Sizes.Contains(s.Size) && s.inStock));
                }
                if (Season.Any())
                {
                    Query.Where(p => Season.Contains(p.Season.Name_en.ToLower()));
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Replace("_", " ");
                    switch (filterDTO.Language)
                    {
                        case "en":
                            Query.Where(p => p.Name_en.ToLower().Contains(search.ToLower()));
                            break;
                        case "uk":
                            Query.Where(p => p.Name_uk.ToLower().Contains(search.ToLower()));
                            break;
                        case "es":
                            Query.Where(p => p.Name_es.ToLower().Contains(search.ToLower()));
                            break;
                        case "fr":
                            Query.Where(p => p.Name_fr.ToLower().Contains(search.ToLower()));
                            break;
                        default:
                            Query.Where(p => p.Name_en.ToLower().Contains(search.ToLower()));
                            break;
                    }
                }
                switch (sort)
                {
                    case "newest":
                        Query.OrderByDescending(p => p.Id);
                        break;
                    case "most_popular":
                        Query.OrderByDescending(p => p.OrderItems.GroupBy(oi => oi.ProductId).Select(g => g.Sum(oi => oi.Quantity)).Sum());
                        break;
                    case "price_low_to_high":
                        Query.OrderBy(p => p.Price);
                        break;
                    case "price_high_to_low":
                        Query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        Query.OrderByDescending(p => p.Id);
                        break;
                }

                Query.Skip((page - 1) * pageSize)
                     .Take(pageSize);
            }
        }
        public class ProductQuantityByFiltersAsync : Specification<ProductEntity>
        {
            public ProductQuantityByFiltersAsync(FilterDTO? filterDTO)
            {
                List<string> Colors = filterDTO.Color ?? new List<string>();
                List<string> Materials = filterDTO.Material ?? new List<string>();
                List<string> Sizes = filterDTO.Size ?? new List<string>();
                List<string> Season = filterDTO.Season ?? new List<string>();
                string search = filterDTO.Search;

                Query.OrderBy(p => p.Id)
                     .Include(p => p.Images)
                     .Include(p => p.Category)
                     .Include(p => p.Storages)
                     .Include(p => p.Color)
                     .Include(p => p.Season)
                     .Include(p => p.Material)
                     .Where(p => p.Storages != null && p.Storages.Any(s => s.inStock));

                if (!string.IsNullOrEmpty(filterDTO.MainCategory))
                {
                    Query.Where(p => p.Category.SubCategory.MainCategory.URLName == filterDTO.MainCategory);
                }
                if (filterDTO.urlName != null)
                {
                    Query.Where(p => filterDTO.urlName.Contains(p.Category.URLName));
                }
                if (filterDTO.subName != null)
                {
                    Query.Where(p => filterDTO.subName.Contains(p.Category.SubCategory.URLName));
                }
                if (Colors.Any())
                {
                    Query.Where(p => Colors.Contains(p.Color.Name_en.ToLower()));
                }

                if (Materials.Any())
                {
                    Query.Where(p => Materials.Contains(p.Material.Name_en.ToLower()));
                }
                if (Sizes.Any())
                {
                    Query.Where(p => p.Storages != null && p.Storages.Any(s => Sizes.Contains(s.Size) && s.inStock));
                }
                if (Season.Any())
                {
                    Query.Where(p => Season.Contains(p.Season.Name_en.ToLower()));
                }
                if (!string.IsNullOrEmpty(search))
                {
                    search = search.Replace("_", " ");
                    switch (filterDTO.Language)
                    {
                        case "en":
                            Query.Where(p => p.Name_en.ToLower().Contains(search.ToLower()));
                            break;
                        case "uk":
                            Query.Where(p => p.Name_uk.ToLower().Contains(search.ToLower()));
                            break;
                        case "es":
                            Query.Where(p => p.Name_es.ToLower().Contains(search.ToLower()));
                            break;
                        case "fr":
                            Query.Where(p => p.Name_fr.ToLower().Contains(search.ToLower()));
                            break;
                        default:
                            Query.Where(p => p.Name_en.ToLower().Contains(search.ToLower()));
                            break;
                    }
                }
                var results = Query;
            }
        }
        public class GetProductById : Specification<ProductEntity>
        {
            public GetProductById(int id)
            {
                Query
                    .Where(p => p.Id == id)
                    .Include(p => p.Images)
                    .Include(p => p.Storages.OrderBy(s => s.SortOrder))
                    .Include(p => p.Color)
                    .Include(p => p.Season)
                    .Include(p => p.Material)
                    .Include(p => p.Category)
                    .ThenInclude(p => p.SubCategory)
                    .ThenInclude(p => p.MainCategory);
            }
        }
        public class GetProductByUrlName : Specification<ProductEntity>
        {
            public GetProductByUrlName(string urlName)
            {
                Query
                    .Where(p => p.Category!.URLName == urlName)
                    .Include(p => p.Images)
                    .Include(p => p.Color)
                    .Include(p => p.Season)
                    .Include(p => p.Material)
                    .Include(p => p.Category)
                    .ThenInclude(p => p.SubCategory)
                    .ThenInclude(p => p.MainCategory);
            }
        }
    }
}