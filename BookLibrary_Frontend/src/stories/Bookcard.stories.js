import React from "react";
import { MemoryRouter } from "react-router-dom";
import Bookcard from "../components/Bookcard";
import "../bootstrap.min.css";

export default {
  title: "Bookcard",
  component: Bookcard,
  decorators: [
    (Story) => (
      <MemoryRouter>
        <Story />
      </MemoryRouter>
    ),
  ],
};
const Template = (args) => <Bookcard {...args} />;

export const Default = Template.bind({});

Default.args = {
  book: {
    name: "a",
    id: "2",
  },
};
