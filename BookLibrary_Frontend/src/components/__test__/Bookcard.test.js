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
  const { getByTestId } = render(
    <Bookcard book={{ name: "book", id: "1" }} />,
    {
      wrapper: MemoryRouter,
    }
  );

  const titleEl = getByTestId("title");

  const subtitleEl = getByTestId("subTitle");

  expect(titleEl.textContent).toBe("book");
  expect(subtitleEl.textContent).toBe("1");
});
