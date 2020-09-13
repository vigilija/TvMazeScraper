## TvMaze Scraper

Application scrapes the TVMaze scrapes the TVMaze API for show and cast information.

__Data are stored in MSSQLLocalDB.__

First time when using application run command in Package Manager Console:
_PM> Update-Database_

New Db will be created from Migrations folder snapshots file.
When service is runing DB is constatntly updated with new data from TVMaze API.  

__Application provides a paginated list of all tv shows containing the id of the TV show and a list ofall the cast that are playing in that TV show.__

Example: https://localhost:44369/api/shows?Page=2

__The list of the cast is ordered by birthday descending.__

If birthday is not known it shows "0001-01-01T00:00:00" as default value and person is listed at the end of the list.
