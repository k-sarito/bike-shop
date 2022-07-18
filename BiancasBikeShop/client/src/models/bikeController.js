const baseUrl = "/api/Bike";

export const getAllBikes = () => {
    return fetch(`${baseUrl}`).then((res) => res.json());
}