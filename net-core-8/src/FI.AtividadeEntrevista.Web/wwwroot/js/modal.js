export function mostrarModal(titulo, texto, aoFechar) {
    const elemento = document.createElement('div');
    elemento.className = 'modal fade';
    elemento.tabIndex = -1;
    elemento.innerHTML = `
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"></h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button>
                </div>
                <div class="modal-body">
                    <p class="mensagem mb-0"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>`;

    elemento.querySelector('.modal-title').textContent = titulo;
    elemento.querySelector('.mensagem').textContent = texto;

    elemento.addEventListener('hidden.bs.modal', () => {
        elemento.remove();
        aoFechar?.();
    });

    document.body.append(elemento);
    new bootstrap.Modal(elemento).show();
}
