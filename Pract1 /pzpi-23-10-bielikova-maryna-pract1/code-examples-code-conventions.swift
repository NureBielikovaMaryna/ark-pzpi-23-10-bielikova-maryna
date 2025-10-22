//В.1 Приклад оформлення програмного коду
// Гарний приклад
final class AuthService {
    private let network: NetworkClient

    init(network: NetworkClient) {
        self.network = network
    }

    func login(email: String, password: String, completion: @escaping (Result<User, Error>) -> Void) {
        let request = LoginRequest(email: email, password: password)
       network.send(request, completion: completion)
    }
}
//Поганий приклад
 class A {
    func b(_ x: String) {
        // ...
    }
}


// В.2 Приклад документування програмного коду 
1 /// Обчислює щомісячний платіж.
2 /// - Parameters:
3 /// - principal: сума позики
4 /// - rate: річна процентна ставка
5 /// - Returns: сума платежу за місяць
6 func calculateMonthlyPayment(principal: Double, rate: Double) -> Double {
7 // ...
8 }


