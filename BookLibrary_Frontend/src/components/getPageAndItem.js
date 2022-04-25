const getPageAndItem = (address, type) => {
  const exp = /page=([0-9]+)&item=([0-9]+)/g;
  let [_, page, item] = exp.exec(address);

  if (type == "nextPage") {
    return { page: Number(page) - 1, item: item };
  } else if (type == "prevPage") {
    return { page: Number(page) + 1, item: item };
  }
};

export default getPageAndItem;
