import { useState, useEffect } from "react"
import { getAllBikes } from "../models/bikeController"
import BikeCard from "./BikeCard"

export default function BikeList({setDetailsBikeId}) {
    const [bikes, setBikes] = useState([])

    const getBikes = () => {
        getAllBikes().then(res => setBikes(res));
    }

    useEffect(() => {
        getBikes()
    }, [])
    return (
        <>
        <h2>Bikes</h2>
        <div className="bike-cards">
        {bikes.map(bike => {
            return <BikeCard key={bike.id} bike={bike}/>
        })}

        </div>
        </>
    )
}