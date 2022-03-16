import React from "react";
import Bookcard from "../Bookcard";
import { render, fireEvent, cleanup } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { act } from "react-dom/test-utils";
import { Link, MemoryRouter, Router } from "react-router-dom";

test("book data returns an error without props", () => {
  const { getByTestId } = render(
    <MemoryRouter>
      <Bookcard />
    </MemoryRouter>
  );
  const cardEl = getByTestId("noData");

  expect(cardEl.textContent).toBe("Error 500");
});

test("card shows correct date", () => {
  const data = {
    Id: 2,
    IsAvailable: true,
    Isbn: "101-22258489522",
    Title: "Learn Design Patterns in R",
  };
  const { getByTestId } = render(<Bookcard book={data} />, {
    wrapper: MemoryRouter,
  });

  const titleEl = getByTestId("title");
  const subtitleEl = getByTestId("subTitle");
  const idEl = getByTestId("link");

  expect(titleEl.textContent).toBe("Learn Design Patterns in R");
  expect(subtitleEl.textContent).toBe("101-22258489522");
  expect(idEl.textContent).toBe("More Details");
  expect(idEl.closest("a")).toHaveAttribute("href", "/book/2");
});
