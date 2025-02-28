namespace py order_service

service OrderService {
    string calculateTotal(1: string product_id, 2: i32 quantity)
}