using System;
using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;
using AppRopio.Models.Contacts.Enums;
using AppRopio.Models.Contacts.Responses;

namespace AppRopio.Base.Contacts.API.Services.Implementation
{
    public class FakeContactsService : IContactsService
    {
        public async Task<ListResponse> LoadContacts(Coordinates location)
        {
            return new ListResponse()
            {
                Contacts = new System.Collections.Generic.List<ListResponseItem>{
                    new ListResponseItem {
                        Value = "84952155316",
                        DisplayValue = "8 (495) 215 5316",
                        Type = ContactTypes.Phone,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAu5JREFUWAntlk1IVFEUx//nzYypQxItSgglC1OHNgV+hYmGJFMIQZhBILVpFSG0qFUQLQtaBS2iFhEFBbnIAhESQkvFXISj2acRpdWECDbOzJt3+78nM7iIcO44q+6Fefe8+3HO7/7feecNYJpRwChgFDAKGAWMAkaB/0gB0T2rUp0FmJq9zv1dgBoBCrsl9Pybrr+17tMC9mAjnx4ySEcmkMgMrECrVA99zYzlwbC0fKZhRaLwWQcgmIBSu+AkB9Wbxm1aPte4SQ8YaPf8+/xhqR55ho3+NkK/InQlbPvRGmNrLdMEls9eNJWKu72UvfgF8Z8ktOJtrZo7GPTm83DRAxY17rE4aHJ79a65DA6VVUQWDEhp/1IeWD2XesDKerwC5Bzx+kTsPPud1HoORcHOlbn8XPWAi4tc4CQVbVHvm8qBwBWIxKh1KZaX9+cHdcWrFrBUDC4Q8B5dBBBPXJTQ8CxfuAueS8e5qabrKvIFrVWHXRg107QDyfg000DgQyOqRsYxVddH1cOc/gBLwlIzOrPe4NrAHvRk/VXAOUfoLyhWtYgXLCGVGOBcHc+xSOgeWM4EbNzimhr+7qCm/IzIg4TuQXIDVi1+RH73U+9WAr5EQNoR3GJjYf42U+SYByWSou3LAAqesm4fZSlkzmfftHI4HUZk0IaFTsJ+JFQDks4QFue3Smi0i+qeYIn76cEKlfUHqDp+eCmzmOpTb8Mb0n6y6XMCdgMxT6OAv5VWhDC7kVKvVaS+B9WH7rPEVXJJFw/QLVXDY/D5mwnN/xp8Inb0dDag6bU5A7uOvCpRWLKP1l3eBqGca4g8GUcs1oHQ9t50MH62mRqS9O4VCjPjWRg55fDf4qjJBlaJ1A3OsT67Tb5T0V6INcX+ElOkxMv3zZvadL6I6w7sInr5aUePE+4s02SvO5ZpAjdVTrGWL2fGsjDyArw6vpps3AOxDxPcfenGJDR2efW8sY0CRgGjgFHAKGAUMAoYBf6hwB8FveQFcUucvwAAAABJRU5ErkJggg=="
                    },
                    new ListResponseItem {
                        Value = "88127482096",
                        DisplayValue = "8 (812) 748 2096",
                        Type = ContactTypes.Phone,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAu5JREFUWAntlk1IVFEUx//nzYypQxItSgglC1OHNgV+hYmGJFMIQZhBILVpFSG0qFUQLQtaBS2iFhEFBbnIAhESQkvFXISj2acRpdWECDbOzJt3+78nM7iIcO44q+6Fefe8+3HO7/7feecNYJpRwChgFDAKGAWMAkaB/0gB0T2rUp0FmJq9zv1dgBoBCrsl9Pybrr+17tMC9mAjnx4ySEcmkMgMrECrVA99zYzlwbC0fKZhRaLwWQcgmIBSu+AkB9Wbxm1aPte4SQ8YaPf8+/xhqR55ho3+NkK/InQlbPvRGmNrLdMEls9eNJWKu72UvfgF8Z8ktOJtrZo7GPTm83DRAxY17rE4aHJ79a65DA6VVUQWDEhp/1IeWD2XesDKerwC5Bzx+kTsPPud1HoORcHOlbn8XPWAi4tc4CQVbVHvm8qBwBWIxKh1KZaX9+cHdcWrFrBUDC4Q8B5dBBBPXJTQ8CxfuAueS8e5qabrKvIFrVWHXRg107QDyfg000DgQyOqRsYxVddH1cOc/gBLwlIzOrPe4NrAHvRk/VXAOUfoLyhWtYgXLCGVGOBcHc+xSOgeWM4EbNzimhr+7qCm/IzIg4TuQXIDVi1+RH73U+9WAr5EQNoR3GJjYf42U+SYByWSou3LAAqesm4fZSlkzmfftHI4HUZk0IaFTsJ+JFQDks4QFue3Smi0i+qeYIn76cEKlfUHqDp+eCmzmOpTb8Mb0n6y6XMCdgMxT6OAv5VWhDC7kVKvVaS+B9WH7rPEVXJJFw/QLVXDY/D5mwnN/xp8Inb0dDag6bU5A7uOvCpRWLKP1l3eBqGca4g8GUcs1oHQ9t50MH62mRqS9O4VCjPjWRg55fDf4qjJBlaJ1A3OsT67Tb5T0V6INcX+ElOkxMv3zZvadL6I6w7sInr5aUePE+4s02SvO5ZpAjdVTrGWL2fGsjDyArw6vpps3AOxDxPcfenGJDR2efW8sY0CRgGjgFHAKGAUMAoYBf6hwB8FveQFcUucvwAAAABJRU5ErkJggg=="
                    },
                    new ListResponseItem {
                        DisplayValue = "info@appropio.com",
                        Value = "info@appropio.com",
                        Type = ContactTypes.Email,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAe1JREFUWAntlD8sA3EUx9+7VoNETBZDLRLaIjG0JTWJSSVMRrHoIhIxk9jESGwWiVUiEjoZLERr8qclxGI0GSyu6T3v1C+51LXudy3Tu+Tud/fu/b7v2897VwA5hIAQEAJCQAgIASEgBIRA0wigrhIVE8dAlNbd55qPmMVoXkvLcBWqF2yWWbsG0WS9Um7vgm5BLzGMXWl3x6lLhTg5n73e6xP+VqbnsbDXItV59JDqq455ffZtGD7MW7qPz3stZOcRZVqoGF8Fy7zW2efM1W7rj1YiHkEomMHei1encPU9FZMjANYuEAwA8pX4yofuaPknjMYc13tjbNNglu7432Om2qT9zO3voEJim82eV8ziE2Bg3C3XS8y3YYzm9iFkDDKnUzbSxcYP2fQePU90qsJ0n5iCsllg20ucU+b4BrS1D2Hk8kzl6K6+R0K1kogQHpKLYNEmF2/n8wUMY4V/wOzXWXGUh4CxgP25G2VQjZbSUfHfVt+ElTAiEkbyO2DgMCDmOB4GyzqomMV3QGMZoulRp1m118/asGFVlE0/QqQnxSOyxrESm89CayjGo7OFuG6pvH9f7VaqdtYqzh9ad613Ku5FR+U6V98z7BRp5P7vZ9hudbMOxJNmSYmOEBACQkAICAEhIASEgBDwQeAT+qqWssLqMLoAAAAASUVORK5CYII="
                    },
                    new ListResponseItem {
                        DisplayValue = "appropio.com",
                        Value = "http://appropio.com",
                        Type = ContactTypes.Url,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAytJREFUWAntWEtoE1EUPdM0bUlsq/1YW1pRwboVrS787HTlQkQRBAXdiaA760J0o+5culEEwZ0rPyAqCO5cqKg7P6jF1lo1Ym1t0k/SxnN6O9SmISkvYyAwFyZv3mfuO3PeufdO62VpqCCrqiCss1BDwP/7xEKGQ4ZzGAglkUNI4N2Q4cApzXEYMpxDSODd6sA8Jp8Do4+B1EsgnQA8flNVxYDYZmD5XiC+JZCtvJK/1qYGgKFLQJJAC1l8E9B+FqjpKrSq6FxpGk69AvqOGdhIHGjlfU27bVpVB9TvAFoOAZrTC2lt6nVRUIUWuDMsZgVgehRY1kP2zgOJq8Dv+0C0BVhzjW2n7Z3+Bny9QNCUjcCvvenMtDvDkoHAeh4w+ydAChh5xH4UWH1lHqwgR1cBbadsbjoJDJ4pRGLBObegE1M64giDyuOVfAF8JNvZDNC4G6hdN7/pVD8wfBv4dcvmvQgw/sHYdghEN8DKBrLmIwS4h0F3ERh7ZmOjTwhoH1+mntniJ5DhJfN4mM2HmTmo7cR1yyhlA6zUJavfacctCbzdbgxqfGpQv2YC3rALWLEfqOsGJt8bYN+Hv26JrRvDyrOy6EprMz+AmTT7bcD6O2T2O/X9h30GX6TZ1vi/1Y125/vwx5fYOgZdxtx/YV7NEugCo0ajHWRzw2KwMwzM/t651XM+FjxbvOMGONpqnscYfEptmWFqs5YtmV/0AnMglLM/UfPjb2zAP53iGBescAMc22pOdLwT1GTfUfbJbHaGwfeU99O8yGCaWh55QFZPAp+PU9sDJhPOIuZWqt00rCBSqlIaa2IwDd+lhnncsoHTlhGUnP/9H42yQ9MBFhY+J5MPB3NjWOlI3wYqApP9QDdZbDtBWdCdCgnkllzUdLIKbgM6ztmaiXf2jJ51SGl6v2BKszYXqCWV5gaW5hvOpdkdsF5XgSQJqETrG6HpIDX7kFodsgIR72HV66Jk7hmzEYLtukz9btTTTlYaYG1Z5s/L0gH7PFXMB7wPuEytW5YoE7h824SA87ES5FjIcJBs5vMVMpyPlSDHQoaDZDOfr5DhfKwEOVZxDP8FErcE1cBGSAIAAAAASUVORK5CYII="
                    },
                    new ListResponseItem {
                        DisplayValue = "AppRopio в Facebook",
                        Value = "https://www.facebook.com/appropio",
                        Type = ContactTypes.Url,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAytJREFUWAntWEtoE1EUPdM0bUlsq/1YW1pRwboVrS787HTlQkQRBAXdiaA760J0o+5culEEwZ0rPyAqCO5cqKg7P6jF1lo1Ym1t0k/SxnN6O9SmISkvYyAwFyZv3mfuO3PeufdO62VpqCCrqiCss1BDwP/7xEKGQ4ZzGAglkUNI4N2Q4cApzXEYMpxDSODd6sA8Jp8Do4+B1EsgnQA8flNVxYDYZmD5XiC+JZCtvJK/1qYGgKFLQJJAC1l8E9B+FqjpKrSq6FxpGk69AvqOGdhIHGjlfU27bVpVB9TvAFoOAZrTC2lt6nVRUIUWuDMsZgVgehRY1kP2zgOJq8Dv+0C0BVhzjW2n7Z3+Bny9QNCUjcCvvenMtDvDkoHAeh4w+ydAChh5xH4UWH1lHqwgR1cBbadsbjoJDJ4pRGLBObegE1M64giDyuOVfAF8JNvZDNC4G6hdN7/pVD8wfBv4dcvmvQgw/sHYdghEN8DKBrLmIwS4h0F3ERh7ZmOjTwhoH1+mntniJ5DhJfN4mM2HmTmo7cR1yyhlA6zUJavfacctCbzdbgxqfGpQv2YC3rALWLEfqOsGJt8bYN+Hv26JrRvDyrOy6EprMz+AmTT7bcD6O2T2O/X9h30GX6TZ1vi/1Y125/vwx5fYOgZdxtx/YV7NEugCo0ajHWRzw2KwMwzM/t651XM+FjxbvOMGONpqnscYfEptmWFqs5YtmV/0AnMglLM/UfPjb2zAP53iGBescAMc22pOdLwT1GTfUfbJbHaGwfeU99O8yGCaWh55QFZPAp+PU9sDJhPOIuZWqt00rCBSqlIaa2IwDd+lhnncsoHTlhGUnP/9H42yQ9MBFhY+J5MPB3NjWOlI3wYqApP9QDdZbDtBWdCdCgnkllzUdLIKbgM6ztmaiXf2jJ51SGl6v2BKszYXqCWV5gaW5hvOpdkdsF5XgSQJqETrG6HpIDX7kFodsgIR72HV66Jk7hmzEYLtukz9btTTTlYaYG1Z5s/L0gH7PFXMB7wPuEytW5YoE7h824SA87ES5FjIcJBs5vMVMpyPlSDHQoaDZDOfr5DhfKwEOVZxDP8FErcE1cBGSAIAAAAASUVORK5CYII="
                    },
                    new ListResponseItem {
                        DisplayValue = "AppRopio в Вк",
                        Value = "https://vk.com/appropio",
                        Type = ContactTypes.Url,
                        ImageBASE64 = "iVBORw0KGgoAAAANSUhEUgAAACwAAAAsCAYAAAAehFoBAAAABGdBTUEAALGPC/xhBQAAAytJREFUWAntWEtoE1EUPdM0bUlsq/1YW1pRwboVrS787HTlQkQRBAXdiaA760J0o+5culEEwZ0rPyAqCO5cqKg7P6jF1lo1Ym1t0k/SxnN6O9SmISkvYyAwFyZv3mfuO3PeufdO62VpqCCrqiCss1BDwP/7xEKGQ4ZzGAglkUNI4N2Q4cApzXEYMpxDSODd6sA8Jp8Do4+B1EsgnQA8flNVxYDYZmD5XiC+JZCtvJK/1qYGgKFLQJJAC1l8E9B+FqjpKrSq6FxpGk69AvqOGdhIHGjlfU27bVpVB9TvAFoOAZrTC2lt6nVRUIUWuDMsZgVgehRY1kP2zgOJq8Dv+0C0BVhzjW2n7Z3+Bny9QNCUjcCvvenMtDvDkoHAeh4w+ydAChh5xH4UWH1lHqwgR1cBbadsbjoJDJ4pRGLBObegE1M64giDyuOVfAF8JNvZDNC4G6hdN7/pVD8wfBv4dcvmvQgw/sHYdghEN8DKBrLmIwS4h0F3ERh7ZmOjTwhoH1+mntniJ5DhJfN4mM2HmTmo7cR1yyhlA6zUJavfacctCbzdbgxqfGpQv2YC3rALWLEfqOsGJt8bYN+Hv26JrRvDyrOy6EprMz+AmTT7bcD6O2T2O/X9h30GX6TZ1vi/1Y125/vwx5fYOgZdxtx/YV7NEugCo0ajHWRzw2KwMwzM/t651XM+FjxbvOMGONpqnscYfEptmWFqs5YtmV/0AnMglLM/UfPjb2zAP53iGBescAMc22pOdLwT1GTfUfbJbHaGwfeU99O8yGCaWh55QFZPAp+PU9sDJhPOIuZWqt00rCBSqlIaa2IwDd+lhnncsoHTlhGUnP/9H42yQ9MBFhY+J5MPB3NjWOlI3wYqApP9QDdZbDtBWdCdCgnkllzUdLIKbgM6ztmaiXf2jJ51SGl6v2BKszYXqCWV5gaW5hvOpdkdsF5XgSQJqETrG6HpIDX7kFodsgIR72HV66Jk7hmzEYLtukz9btTTTlYaYG1Z5s/L0gH7PFXMB7wPuEytW5YoE7h824SA87ES5FjIcJBs5vMVMpyPlSDHQoaDZDOfr5DhfKwEOVZxDP8FErcE1cBGSAIAAAAASUVORK5CYII="
                    },
                    //new ListResponseItem {
                    //    DisplayValue = "Hello world",
                    //    Value = "Hello world",
                    //    Type = ContactTypes.Text
                    //},
                    new ListResponseItem {
                        DisplayValue = "Офис AppRopio",
                        Value = "СПб. ул. Заозерная, д. 8",
                        Type = ContactTypes.Address
                    }
                }
            };
        }
    }
}
