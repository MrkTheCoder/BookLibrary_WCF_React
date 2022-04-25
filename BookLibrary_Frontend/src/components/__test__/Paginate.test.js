import React from "react";
import Paginate from "../Paginate";
import { render } from "@testing-library/react";
import "@testing-library/jest-dom/extend-expect";
import { MemoryRouter } from "react-router-dom";

test("Element renders without errors", () => {
  const data = {
    totalItems: 21,
    nextPage: "?page=2&item=10",
  };
  const { getByTestId } = render(
    <MemoryRouter>
      <Paginate totalItems={data.totalItems} nextPage={data.nextPage} />
    </MemoryRouter>
  );
});

test("current page has the 'active' className", () => {
  const data = {
    totalItems: 21,
    nextPage: "?page=2&item=10",
  };
  const { getByTestId } = render(
    <MemoryRouter>
      <Paginate totalItems={data.totalItems} nextPage={data.nextPage} />
    </MemoryRouter>
  );

  const currentPAge = getByTestId("currentPage");

  expect(currentPAge.className).toBe("active");
});
