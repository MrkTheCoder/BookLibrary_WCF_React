import { rest } from "msw";

export const handlers = [
  rest.get(
    "http://localhost:51202/api/CategoryManager/categories",
    (req, res, ctx) => {
      return res(
        ctx.json([
          {
            BooksInCategory: 1,
            Name: "1",
          },
          {
            BooksInCategory: 2,
            Name: "2",
          },
          {
            BooksInCategory: 3,
            Name: "3",
          },
          {
            BooksInCategory: 4,
            Name: "4",
          },
        ])
      );
    }
  ),
];
