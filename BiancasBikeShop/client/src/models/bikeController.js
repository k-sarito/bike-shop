const baseUrl = "/api/Bike";

export const getAllBikes = () => {
    return fetch(`${baseUrl}`).then((res) => res.json());
}

export const getBikeById = (id) => {
    return fetch (`${baseUrl}/${id}`).then((res) => res.json());
}